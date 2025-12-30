using BeastBoards.Api.Models;
using BeastBoards.Api.Stubs.Api;
using Microsoft.EntityFrameworkCore;

namespace BeastBoards.Api.Services
{
    public class BeastBoardsLeaderboardService(
        BeastBoardsContext _db    
    )
    {

        public List<LeaderboardTiming> AddLeaderboardTiming(AddLeaderboardTimingRequest req, ulong steamId)
        {
            var exists = _db.LeaderboardTimings.FirstOrDefault(x => x.SteamId == steamId && x.LevelNumber == req.LevelNumber);

            LeaderboardTiming timing = null;
            if (exists != null)
            {
                if (exists.Time > req.BestTime)
                {
                    exists.Time = req.BestTime;
                    _db.LeaderboardTimings.Update(exists);
                    _db.SaveChanges();

                }
                timing = exists;
            }
            else
            {
                //create entry
                var newEntry = new LeaderboardTiming()
                {
                    LevelNumber = req.LevelNumber,
                    SteamId = steamId,
                    Time = req.BestTime
                };

                _db.LeaderboardTimings.Add(newEntry);
                _db.SaveChanges();

                timing = newEntry;
            }


            //get friend entries
            var friends = _db.LeaderboardTimings.AsNoTracking().Where(x => x.LevelNumber == req.LevelNumber && req.FriendIds.Contains(x.SteamId)).ToList();

            var allEntries = friends.Concat([timing]).OrderBy(x => x.Time).ToList();

            return allEntries;
        }

    }
}
