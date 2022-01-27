using System;

namespace Scoreboard
{
    [Serializable]
    public class Player
    {
        public long id;
        public string name;
        public string token;
        public GameRun[] gameRuns;
    }

    [Serializable]
    public class GameRun
    {
        public long id;
        public long playerID;
        public long startTime;
        public long endTime;
        public int kills;
        public int headshots;
        public int deaths;
        public int score;
        public GameRunLevel[] levels;
    }

    [Serializable]
    public class GameRunLevel
    {
        public int id;
        public int gameRunID;
        public long startTime;
        public long endTime;
        public int level;
        public int kills;
        public int headshots;
        public int deaths;
        public int score;
    }
}
