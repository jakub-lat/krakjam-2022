using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Scoreboard
{
    public class ElevatorScoreboard : MonoBehaviour
    {
        [SerializeField] private GameObject surface;
        [SerializeField] private GameObject canvas;
        [SerializeField] private GameObject table;
        [SerializeField] private Text loadingText;
        [SerializeField] private Text errorText;
        [SerializeField] private Text idCol;
        [SerializeField] private Text nameCol;
        [SerializeField] private Text scoreCol;

        [SerializeField] private Text timeCol;

        // [SerializeField] private Text headshotsCol;
        [SerializeField] private Text deathsCol;

        public void Hide()
        {
            surface.SetActive(false);
            canvas.SetActive(false);
        }

        public async Task Show(int level)
        {
            surface.SetActive(true);
            canvas.SetActive(true);

            table.SetActive(false);
            loadingText.enabled = true;
            try
            {
                var data = await GameScoreboard.Current.GetScoreboardForLevel(level, 15);
                // Debug.Log(JsonConvert.SerializeObject(data));
                table.SetActive(true);
                loadingText.enabled = false;
                RenderData(data);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                loadingText.enabled = false;
                errorText.enabled = true;
            }
        }

        private string GetTitleText(string str)
        {
            return $"<size=6><color=#df1640>{str}</color></size>";
        }

        private string Colorize(string str, bool sure)
        {
            return sure ? $"<color=green>{str}</color>" : str;
        }

        public static string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        private void RenderRow(GameRunLevel x, bool isCurrent)
        {
            idCol.text += "\n" + Colorize(x.position.ToString(), isCurrent);
            nameCol.text += "\n" + Colorize(Truncate(x.player?.name.ToUpper() ?? "", 12), isCurrent);
            scoreCol.text += "\n" + Colorize(x.score.ToString(), isCurrent);
            timeCol.text += "\n" + Colorize(TimeSpan.FromSeconds(x.endTime - x.startTime).ToString(@"mm\:ss"),
                isCurrent);
            // headshotsCol.text += "\n" + Colorize(x.headshots.ToString(), isCurrent);
            deathsCol.text += "\n" + Colorize(x.deaths.ToString(), isCurrent);
        }

        private void RenderData(ScoreboardForLevelResponse data)
        {
            idCol.text = GetTitleText("-");
            nameCol.text = GetTitleText("NAME");
            scoreCol.text = GetTitleText("SCORE");
            timeCol.text = GetTitleText("TIME");
            // headshotsCol.text = GetTitleText("HEADSHOTS");
            deathsCol.text = GetTitleText("DEATHS");

            var wasCurrent = false;
            foreach (var (x, i) in data.others.GetRange(0, Math.Min(15, data.others.Count)).Select((x, i) => (x, i)))
            {
                if (x == null)
                {
                    Debug.LogError($"gameRunLevel on index {i} is null! skipping");
                    continue;
                }

                var isCurrent = x.playerID == GameScoreboard.Current.runData.playerID;
                if (isCurrent)
                {
                    wasCurrent = true;
                }

                RenderRow(x, isCurrent);
            }

            if (wasCurrent) return;

            if (data.player == null)
            {
                Debug.LogError("data.player is null!");
                Debug.LogError(JsonConvert.SerializeObject(data));
                return;
            }


            RenderRow(data.player, true);
        }
    }
}
