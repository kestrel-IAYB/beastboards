namespace BeastBoards.Common.Stubs.Api
{
    public class AddLeaderboardTimingRequest
    {
        public List<ulong> FriendIds { get; set; }
        public int LevelNumber { get; set; }
        public string Category { get; set; }
        public float BestTime { get; set; }
    }
}

