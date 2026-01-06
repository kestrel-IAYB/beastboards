using BeastBoards.Common.Stubs;
using Steamworks;

namespace BeastBoards.Common
{
    public class Steam
    {
        public string SteamToken { get; set; }

        public CSteamID UserId { get; set; }

        public List<SteamUserStub> Users { get; set; }

        public Steam(GetTicketForWebApiResponse_t result)
        {
            SteamToken = BitConverter.ToString(result.m_rgubTicket).Replace("-", "");
            Users = new List<SteamUserStub>();


            var friendsCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);

            //create steam friends
            for (int i = 0; i < friendsCount; i++)
            {
                var friend = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);

                var avatar = SteamFriends.GetSmallFriendAvatar(friend);
                Users.Add(new SteamUserStub()
                {
                    Id = friend.m_SteamID,
                    Avatar = avatar,
                    AvatarTexture = Utilities.GetSteamImageAsTexture2D(avatar),
                    IsPlayer = false,
                    PersonaName = SteamFriends.GetFriendPersonaName(friend),
                });
            }

            //add player too
            UserId = SteamUser.GetSteamID();
            var playerAvatar = SteamFriends.GetSmallFriendAvatar(UserId);
            Users.Add(new SteamUserStub()
            {
                Id = UserId.m_SteamID,
                Avatar = playerAvatar,
                AvatarTexture = Utilities.GetSteamImageAsTexture2D(playerAvatar),
                IsPlayer = true,
                PersonaName = SteamFriends.GetPersonaName()
            });


        }

    }
}
