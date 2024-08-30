using AD3D_SubnauticaGitManager.Items.Menu;
using HarmonyLib;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AD3D_SubnauticaGitManager.Patches
{
    [HarmonyPatch(typeof(uGUI_MainMenu))]
    internal class MenuPatcher
    {
        [HarmonyPatch(nameof(uGUI_MainMenu.Awake))]
        [HarmonyPostfix]
        private static void AwakePatch(uGUI_MainMenu __instance)
        {
            var playButton = __instance.gameObject.GetComponentInChildren<MainMenuPrimaryOptionsMenu>().transform.Find("PrimaryOptions/MenuButtons/ButtonPlay").gameObject;
            var modManagerButton = Object.Instantiate(playButton);
            modManagerButton.GetComponent<RectTransform>().SetParent(playButton.transform.parent, false);
            modManagerButton.name = "ButtonSGMMenu";
            var text = modManagerButton.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Github Mod Menu";
            Object.DestroyImmediate(text.gameObject.GetComponent<TranslationLiveUpdate>());
            //text.gameObject.AddComponent<TranslatableText>().languageKey = "ModManagerButton";
            modManagerButton.transform.SetSiblingIndex(1);
            var button = modManagerButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();// = new Button.ButtonClickedEvent();
            button.onClick.AddListener(SGMMenuHandler.Show);
        }
    }
}
