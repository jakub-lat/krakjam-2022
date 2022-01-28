using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cyberultimate.Unity;
using Game;
using Unity.VisualScripting.IonicZip;
using UnityEngine;

namespace Scoreboard
{
    public class Scoreboard : MonoSingleton<Scoreboard>
    {
        private const string BaseUrl = "https://krakjam2022scoreboard.cubepotato.eu";

        [SerializeField] private TextAsset configFile;
        private string Secret => configFile.text;

        private const string TokenKey = "SCOREBOARD_TOKEN";
        private string Token => PlayerPrefs.GetString(TokenKey);
        private bool LoggedIn => !string.IsNullOrEmpty(Token);


        public GameRun runData;
        public GameRunLevel levelData;


        public async void Register(string name)
        {
            using var client = new HttpClient();

            var body = JsonUtility.ToJson(new { name });
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
            var res = await client.GetStringAsync(BaseUrl);
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
            res.EnsureSuccessStatusCode();

            return JsonUtility.FromJson<T>(await res.Content.ReadAsStringAsync());
        }
    }
}
