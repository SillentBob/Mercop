using System;

namespace Mercop.Core.Web.Data
{
    [Serializable]
    public class LeaderboardsData
    {
        public LeaderboardsGroup group;

    }

    [Serializable]
    public class LeaderboardsGroup
    {
        public int week;
        public string start;
        public string end;
        public LeaderboardsPlayerData[] players;
    }

    [Serializable]
    public class LeaderboardsPlayerData
    {
        public string uid;
        public string name;
        public LeaderboardsPlayerScores scores;
    }

    [Serializable]
    public class LeaderboardsPlayerScores
    {
        public int current;
        public int past;
    }
    
}