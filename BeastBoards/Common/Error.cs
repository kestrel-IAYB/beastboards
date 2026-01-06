using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeastBoards.Common
{
    public static class Error
    {
        public static GameObject ErrorMessage { get; set; } = null;

        public static void ShowError(string message)
        {
            if (ErrorMessage == null)
            {
                CloseError();
            }

            AssetBundle localAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "beastboardsui"));

            if (localAssetBundle == null)
            {
                return;
            }

            GameObject asset = localAssetBundle.LoadAsset<GameObject>("BeastBoardsMessageCanvas");
            ErrorMessage = GameObject.Instantiate(asset);

            var text = ErrorMessage.transform.Find("Panel/Message");
            var button = ErrorMessage.transform.Find("Panel/Button");

            text.GetComponent<TextMeshProUGUI>().text = message;

            button.GetComponent<Button>().onClick.AddListener(CloseError);


            localAssetBundle.Unload(false);
        }

        public static void CloseError()
        {
            GameManager.Destroy(ErrorMessage);

            ErrorMessage = null;
        }
    }
}
