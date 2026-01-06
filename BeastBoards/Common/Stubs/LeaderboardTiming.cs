namespace BeastBoards.Common.Stubs
{
    public class LeaderboardTiming
    {
        public int Id { get; set; }
        public float Time { get; set; }
        public int LevelNumber { get; set; }
        public string Category { get; set; }
        public ulong SteamId { get; set; }

        public SteamUserStub GetSteamUser()
        {
            return Core.Steam.Users.FirstOrDefault(x => x.Id == SteamId);
        }
    }
}