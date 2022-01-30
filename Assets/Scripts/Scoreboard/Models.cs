using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Scoreboard
{
    [Serializable]
    public class ScoreboardForLevelResponse
    {
        public GameRunLevel player;
        public List<GameRunLevel> others;
    }

    [Serializable]
    public class ScoreboardResponse
    {
        public GameRun player;
        public List<GameRun> others;
    }
    
    [Serializable]
    public class Player
    {
        public long id;
        public string name;
        public string token;
        public List<GameRun> gameRuns;
    }

    [Serializable]
    public class GameRun
    {
        public long id;
        public int position;
        public int mode;
        public long playerID;
        public long startTime;
        public long endTime;
        public int kills;
        public int headshots;
        public int deaths;
        public int score;
        public List<GameRunLevel> levels;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Player player;
    }

    [Serializable]
    public class GameRunLevel
    {
        public int position;
        public long id;
        public long playerID;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Player player;
        public long gameRunID;
        public long startTime;
        public long endTime;
        public int level;
        public int kills;
        public int headshots;
        public int deaths;
        public int score;
    }
}
