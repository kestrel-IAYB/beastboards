using BeastBoards.Common.Stubs;
using HarmonyLib;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeastBoards.Common
{
    public static class Core
    {
        public const string MOD_NAME = "BeastBoards";
        public const string MOD_VERSION = "1.0.0";
        public const string MOD_AUTHOR = "VideoGamesAreBad";
        public const string MOD_GUID = "uk.co.videogamesarebad.beastboards"; // for BepInEx
    
        public static Steam Steam { get; set; } = null;
        public static Api Api { get; set; } = null;
    
        public static bool BeastBoardsIsRunning { get; set; } = false;

        public static void Init()
        {
            try {
                Callback<GetTicketForWebApiResponse_t>.Create(OnGetTicketForWebApiResponse);
                SteamUser.GetAuthTicketForWebApi("BEASTBOARDS");
            }
            catch (InvalidOperationException) {
                Error.ShowError("Error: SteamWorks is not initialized. BeastBoards will not work. Please make sure you are launching the game via Steam.");
            }
        }
    
        private static void OnGetTicketForWebApiResponse(GetTicketForWebApiResponse_t result)
        {
            Steam = new Steam(result);
            Api = new Api();
        }
    
        [HarmonyPatch(typeof(UILevelCompleteScreen), "DisplayLevelNameComplete", [])]
        public static class CompleteLevelPatch
        {
            private static void Postfix()
            {
                if (!BeastBoardsIsRunning)
                {
                    Error.ShowError("Error: Failed to connect to the BeastBoards server. Please check your internet connection and restart the game.");
                    return;
                }

                var info = GameManager.instance.levelController.GetInformationSetter().GetInformation();
                var data = GameManager.instance.progressManager.GetLevelData(info);

                List<LeaderboardTiming> leaderboard = Api.AddLeaderboardTiming(info.GetLevelNumber(), data.GetBestTime(), info.GetLevelCategoryName());
            
                AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "beastboardsui"));
            
                if (localAssetBundle == null)
                {
                    return;
                }

                GameObject asset = localAssetBundle.LoadAsset<GameObject>("BeastBoardsCanvas");
                var created = GameObject.Instantiate(asset);
                var parent = created.transform.Find("BeastBoardsContainer/Leaderboard/Items");

            
                GameObject item = localAssetBundle.LoadAsset<GameObject>("BeastBoardItem");



                foreach (var friend in leaderboard)
                {
                    var entry = GameObject.Instantiate(item, parent.transform, false);

                    var img = entry.transform.Find("Image").GetComponent<RawImage>();
                    img.texture = friend.GetSteamUser().AvatarTexture;


                    var name = entry.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                    name.text = friend.GetSteamUser().PersonaName;

                    var time = entry.transform.Find("Time").GetComponent<TextMeshProUGUI>();
                    time.text = friend.Time.ToString("F2");

                }

                var scrollRect = created.transform.Find("BeastBoardsContainer/Leaderboard").GetComponent<ScrollRect>();
                scrollRect.verticalNormalizedPosition = scrollRect.flexibleHeight;


                localAssetBundle.Unload(false);
            }
        }
    }
}