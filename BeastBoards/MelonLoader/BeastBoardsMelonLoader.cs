using MelonLoader;
using BeastBoards.Common;
using BeastBoards.MelonLoader;
using Steamworks;

[assembly: MelonInfo(typeof(BeastBoardsMelonLoader), Core.MOD_NAME, Core.MOD_VERSION, Core.MOD_AUTHOR)]
[assembly: MelonGame("Strange Scaffold", "I Am Your Beast")]
namespace BeastBoards.MelonLoader
{
    public class BeastBoardsMelonLoader : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Hello from BeastBoards!");
        }

        public override void OnLateInitializeMelon()
		{
            Core.Init();
        }
        
        public override void OnUpdate()
        {
            SteamAPI.RunCallbacks();
        }

    }
}