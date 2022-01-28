using System;
using System.Collections.Generic;

namespace Scoreboard
{
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
        public long playerID;
        public long startTime;
        public long endTime;
        public int kills;
        public int headshots;
        public int deaths;
        public int score;
        public List<GameRunLevel> levels;
    }

    [Serializable]
    public class GameRunLevel
    {
        public long id;
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
