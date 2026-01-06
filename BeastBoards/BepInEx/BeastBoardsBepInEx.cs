using System.Collections;
using BeastBoards.Common;
using BepInEx;
using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace BeastBoards.BepInEx
{
    [BepInPlugin(Core.MOD_GUID, Core.MOD_NAME, Core.MOD_VERSION)]
    public class BeastBoardsBepInEx : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("Hello from BeastBoards!");
			new Harmony(Core.MOD_GUID).PatchAll();
        }
        
        private IEnumerator Start()
        {
            // Wait for all other start calls to finish
            yield return new WaitForEndOfFrame();
            Core.Init();
        }

        private void Update()
        {
            SteamAPI.RunCallbacks();
        }
    }
}