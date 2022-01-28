using System;
using System.Collections.Generic;
using System.Net.Http;
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
        private const string BaseUrl = "https://krakjam2022scoreboard.cubepotato.eu"; // https://krakjam2022scoreboard.cubepotato.eu

        [SerializeField] private TextAsset configFile;
        private string Secret => configFile.text.Trim();

        public const string TokenKey = "SCOREBOARD_TOKEN";
        private string Token => PlayerPrefs.GetString(TokenKey);
        public bool LoggedIn => !string.IsNullOrEmpty(Token);


        public GameRun runData = new();
        public GameRunLevel levelData = new();


        [Serializable]
        private struct RegisterBody
        {
            public string name;
        }

        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"token: {Token}");
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
            };
            runData = await PostData("/run", runData);
            // Debug.Log("SCOREBOARD: new run: "+JsonConvert.SerializeObject(runData));
            levelData = new GameRunLevel
            {
                startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                level = LevelManager.Current.CurrentLevel,
            };
        }

        public async Task PostLevelData()
        {
            levelData.endTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            levelData.gameRunID = runData.id;
            var res = await PostData("/level", levelData);
            // Debug.Log("SCOREBOARD: new level: "+JsonConvert.SerializeObject(res));
            levelData = new GameRunLevel
            {
                level = LevelManager.Current.CurrentLevel,
                startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }

        public async Task<List<Player>> GetScoreboard()
        {
            using var client = new HttpClient();
            var res = await client.GetStringAsync(BaseUrl);
            return JsonConvert.DeserializeObject<List<Player>>(res);
        }
        
        public async Task<ScoreboardForLevelResponse> GetScoreboardForLevel(int level, int count)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);
            var res = await client.GetStringAsync(BaseUrl + $"/level/{level}?limit={count}");
            return JsonConvert.DeserializeObject<ScoreboardForLevelResponse>(res);
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
