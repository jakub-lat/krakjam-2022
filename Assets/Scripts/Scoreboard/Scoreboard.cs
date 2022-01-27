using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cyberultimate.Unity;
using Unity.VisualScripting.IonicZip;
using UnityEngine;

namespace Scoreboard
{
    public class Scoreboard : MonoSingleton<Scoreboard>
    {
        private const string BaseUrl = "https://krakjam2022scoreboard.cubepotato.eu";
        private const string Secret = "vUtrRzVaLfp4PPM";

        private const string TokenKey = "SCOREBOARD_TOKEN";
        private string Token => PlayerPrefs.GetString(TokenKey);
        

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
        
        public async Task<List<Player>> GetScoreboard()
        {
            using var client = new HttpClient();
            var res = await client.GetStringAsync(BaseUrl);
            return JsonUtility.FromJson<List<Player>>(res);
        }
        
        public async Task<Player> GetCurrentPlayer()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);
            var res = await client.GetStringAsync(BaseUrl);
            return JsonUtility.FromJson<Player>(res);
        }

        public async void PostData<T>(string path, T data)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", Token);

            var body = JsonUtility.ToJson(data);

            client.DefaultRequestHeaders.Add("X-Hash", BCrypt.Net.BCrypt.HashPassword(Secret + body));

            var res = await client.PostAsync(BaseUrl + path,
                new StringContent(body, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();
        }
    }
}
