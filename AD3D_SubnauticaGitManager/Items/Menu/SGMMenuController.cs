using AD3D_Common.Utils;
using HarmonyLib;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static VFXParticlesPool;
using Object = UnityEngine.Object;

namespace AD3D_SubnauticaGitManager.Items.Menu
{
    public class SGMMenuController : MonoBehaviour
    {
        public uGUI_MainMenu MainMenu;// { get; private set; }
        public MainMenuPrimaryOptionsMenu PanelParent;// { get; private set; }
        public Button btnCloseMenu;

        private void Awake()
        {
            btnCloseMenu = this.gameObject.FindComponentByName<Button>("btnCloseMenu"); 
            btnCloseMenu.onClick.AddListener(() => ShowMenu(false));

            //MainMenu = uGUI_MainMenu.main ?? Object.FindObjectOfType<uGUI_MainMenu>();
            //PanelParent = MainMenu.gameObject.GetComponentInChildren<MainMenuPrimaryOptionsMenu>();
        }

        public void ShowMenu(bool value)
        {
            // Show it
            this.gameObject.SetActive(value);
            // Hide main
            SetMainMenuShown(!value);
            //Process.Start("SGM\\SubnauticaGithubManager.UI.exe");
        }

        private void SetMainMenuShown(bool shown)
        {
            if (MainMenu == null)
                return;

            PanelParent.gameObject.SetActive(shown);
        }
    }
}
