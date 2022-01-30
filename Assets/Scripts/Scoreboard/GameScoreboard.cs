using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Cyberultimate.Unity;
using Game;
using Newtonsoft.Json;
using UnityEngine;

namespace Scoreboard
{
    public class GameScoreboard : MonoSingleton<GameScoreboard>
    {
        private const string
            BaseUrl = "https://krakjam2022scoreboard.cubepotato.eu"; // https://krakjam2022scoreboard.cubepotato.eu

        // [SerializeField] private TextAsset configFile;
        private string Secret;

        public const string TokenKey = "SCOREBOARD_TOKEN";
        private string Token => PlayerPrefs.GetString(TokenKey);
        public bool LoggedIn => !string.IsNullOrEmpty(Token);


        public bool runDataSet = false;

        public GameRun runData = null;
        public GameRunLevel levelData = null;


        [Serializable]
        private struct RegisterBody
        {
            public string name;
        }

        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"token: {Token}");

            DontDestroyOnLoad(gameObject);

            var t = this.GetType().Assembly.GetType("Scoreboard.ScoreboardConfig");
            if (t != null)
            {
                var f = t.GetFields(
                    BindingFlags.Public | BindingFlags.Static |
                    BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "Secret");
                Secret = (string)f?.GetRawConstantValue();
            }
        }


        public async void Register(string name)
        {
            using var client = new HttpClient();

            var body = JsonConvert.SerializeObject(new RegisterBody { name = name });
            var res = await client.PostAsync(BaseUrl + "/register",
                new StringContent(body, Encoding.UTF8, "application/json"));

            res.EnsureSuccessStatusCode();
            var data = JsonConvert.DeserializeObject<Player>(await res.Content.ReadAsStringAsync());

            PlayerPrefs.SetString(TokenKey, data.token);
            Debug.Log("registered, token: " + data.token);
        }

        public async void NewRun()
        {
            runData = new GameRun
            {
                startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                mode = (int)LevelManager.Current.GameMode,
            };
            runDataSet = true;
            runData = await PostData("/run", runData);
            // Debug.Log("SCOREBOARD: new run: "+JsonConvert.SerializeObject(runData));
            ResetLevelData();
        }

        public async Task PostLevelData()
        {
            levelData.endTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            levelData.gameRunID = runData.id;

            runData.deaths += levelData.deaths;
            runData.kills += levelData.kills;
            runData.headshots += levelData.headshots;
            runData.score += levelData.score;
            runData.endTime = levelData.endTime;

            runData.levels ??= new List<GameRunLevel>();

            runData.levels.Add(levelData);

            var res = await PostData("/level", levelData);
        }

        public GameRun GetCalculatedRunData()
        {
            runData.deaths = levelData.deaths;
            runData.kills = levelData.kills;
            runData.headshots = levelData.headshots;
            runData.score = levelData.score;
            runData.endTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            foreach (var lvl in runData.levels ?? new List<GameRunLevel>())
            {
                runData.deaths += lvl.deaths;
                runData.kills += lvl.kills;
                runData.headshots += lvl.headshots;
                runData.score += lvl.score;
                runData.endTime = lvl.endTime;
            }

            return runData;
        }

        public void ResetLevelData()
        {
            levelData = new GameRunLevel
            {
                level = LevelManager.Current.CurrentLevel,
                startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                score = 0,
            };
        }

        public async Task<ScoreboardResponse> GetScoreboard(int limit)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);
            var res = await client.GetStringAsync(BaseUrl + $"/top?limit={limit}");
            return JsonConvert.DeserializeObject<ScoreboardResponse>(res);
        }

        public async Task<ScoreboardForLevelResponse> GetScoreboardForLevel(int level, int limit)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);
            var res = await client.GetStringAsync(BaseUrl + $"/level/{level}?limit={limit}");
            return JsonConvert.DeserializeObject<ScoreboardForLevelResponse>(res);
        }

        public async Task<GameRun> GetRun(int id)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);
            var res = await client.GetStringAsync(BaseUrl + $"/run/{id}");
            return JsonConvert.DeserializeObject<GameRun>(res);
        }

        public async Task<Player> GetCurrentPlayer()
        {
            if (!LoggedIn) return null;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);
            var res = await client.GetStringAsync(BaseUrl + "/player");
            return JsonConvert.DeserializeObject<Player>(res);
        }

        public async Task<T> PostData<T>(string path, T data)
        {
            if (!LoggedIn) return data;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);

            var body = JsonConvert.SerializeObject(data);

            client.DefaultRequestHeaders.Add("X-Hash", BCrypt.Net.BCrypt.HashPassword(Secret + body));

            var res = await client.PostAsync(BaseUrl + path,
                new StringContent(body, Encoding.UTF8, "application/json"));

            HandleError(res);

            var str = await res.Content.ReadAsStringAsync();
            Debug.Log(typeof(T).Name + ": " + str);
            return JsonConvert.DeserializeObject<T>(str);
        }

        private async void HandleError(HttpResponseMessage res)
        {
            try
            {
                res.EnsureSuccessStatusCode();
            }
            catch
            {
                Debug.LogError(await res.Content.ReadAsStringAsync());
                throw;
            }
        }
    }
}
