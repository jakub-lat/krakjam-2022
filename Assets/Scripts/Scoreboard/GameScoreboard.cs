using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cyberultimate.Unity;
using Game;
using UnityEngine;

namespace Scoreboard
{
    public class GameScoreboard : MonoSingleton<GameScoreboard>
    {
        private const string BaseUrl = "https://krakjam2022scoreboard.cubepotato.eu";

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
        public async void Register(string name)
        {
            using var client = new HttpClient();
            
            var body = JsonUtility.ToJson(new RegisterBody { name = name });
            var res = await client.PostAsync(BaseUrl + "/register",
                new StringContent(body, Encoding.UTF8, "application/json"));
            
            res.EnsureSuccessStatusCode();
            var data = JsonUtility.FromJson<Player>(await res.Content.ReadAsStringAsync());
            
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
            levelData = new GameRunLevel
            {
                startTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                level = LevelManager.Current.CurrentLevel,
            };
        }

        public async void PostLevelData()
        {
            levelData.endTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            levelData.gameRunID = runData.id;
            await PostData("/level", levelData);
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
            return JsonUtility.FromJson<List<Player>>(res);
        }

        public async Task<Player> GetCurrentPlayer()
        {
            if (!LoggedIn) return null;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);
            var res = await client.GetStringAsync(BaseUrl+"/player");
            return JsonUtility.FromJson<Player>(res);
        }

        public async Task<T> PostData<T>(string path, T data)
        {
            if (!LoggedIn) return data;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);

            var body = JsonUtility.ToJson(data);

            client.DefaultRequestHeaders.Add("X-Hash", BCrypt.Net.BCrypt.HashPassword(Secret + body));

            var res = await client.PostAsync(BaseUrl + path,
                new StringContent(body, Encoding.UTF8, "application/json"));

            HandleError(res);

            var str = await res.Content.ReadAsStringAsync();
            return JsonUtility.FromJson<T>(str);
        }

        private void HandleError(HttpResponseMessage res)
        {
            try
            {
                res.EnsureSuccessStatusCode();
            }
            catch
            {
                Debug.LogError(res.Content.ReadAsStringAsync());
                throw;
            }
        }
    }
}
