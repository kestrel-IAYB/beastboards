using UnityEngine;

namespace BeastBoards.Common.Stubs
{
    public class SteamUserStub
    {
        public ulong Id { get; set; }
        public string PersonaName { get; set; }
        public int Avatar { get; set; }
        public Texture2D AvatarTexture { get; set; }
        public bool IsPlayer { get; set; }

    }
}
