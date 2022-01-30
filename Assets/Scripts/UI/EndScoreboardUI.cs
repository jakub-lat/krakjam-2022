using System;
using System.Threading.Tasks;
using Scoreboard;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndScoreboardUI : MonoBehaviour
    {
        [SerializeField] private GameObject table;
        [SerializeField] private Text loadingText;
        [SerializeField] private Text errorText;
        [SerializeField] private Text idCol;
        [SerializeField] private Text nameCol;
        [SerializeField] private Text scoreCol;
        [SerializeField] private Text timeCol;
        [SerializeField] private Text headshotsCol;
        [SerializeField] private Text deathsCol;
        [SerializeField] private Text modeCol;

        private void Start()
        {
            Show();
        }

        public async void Show()
        {
            table.SetActive(false);
            loadingText.enabled = true;
            try
            {
                var data = await GameScoreboard.Current.GetScoreboard(15);
                table.SetActive(true);
                loadingText.enabled = false;
                RenderData(data);
            }
            catch (Exception e)
            {
                Debug.Log(e);
                loadingText.enabled = false;
                errorText.enabled = true;
            }
        }

        private string GetTitleText(string str)
        {
            return $"<size=40><color=#df1640>{str}</color></size>";
        }

        private string Colorize(string str, bool sure)
        {
            return sure ? $"<color=green>{str}</color>" : str;
        }
        
        public static string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        private void RenderRow(GameRun x, bool isCurrent)
        {
            idCol.text += "\n" + Colorize(x.position.ToString(), isCurrent);
            nameCol.text += "\n" + Colorize(Truncate(x.player.name.ToUpper(), 15), isCurrent);
            scoreCol.text += "\n" + Colorize(x.score.ToString(), isCurrent);
            timeCol.text += "\n" + Colorize(TimeSpan.FromSeconds(x.endTime - x.startTime).ToString(@"mm\:ss"), isCurrent);
            // headshotsCol.text += "\n" + Colorize(x.headshots.ToString(), isCurrent);
            deathsCol.text += "\n" + Colorize(x.deaths.ToString(), isCurrent);
        }
        
        private void RenderData(ScoreboardResponse data)
        {
            idCol.text = GetTitleText("-");
            nameCol.text = GetTitleText("NAME");
            modeCol.text = GetTitleText("DIFFICULTY");
            scoreCol.text = GetTitleText("SCORE");
            timeCol.text = GetTitleText("TIME");
            headshotsCol.text = GetTitleText("HEADSHOTS");
            deathsCol.text = GetTitleText("DEATHS");

            var wasCurrent = false;
            foreach (var x in data.others.GetRange(0, Math.Min(15, data.others.Count)))
            {
                var isCurrent = x.playerID == GameScoreboard.Current.runData.playerID;
                wasCurrent = isCurrent;
                RenderRow(x, isCurrent);
            }

            if (!wasCurrent)
            {
                RenderRow(data.player, true);
            }
        }
    }
}
