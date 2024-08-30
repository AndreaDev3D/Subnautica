using AD3D_Common.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_SubnauticaGitManager.Items.Menu
{
    public static class SGMMenuHandler
    {
        public static GameObject MenuObject;// { get; private set; }

        public static uGUI_MainMenu MainMenu;// { get; private set; }
        public static MainMenuPrimaryOptionsMenu PanelParent;// { get; private set; }

        public static void Show()
        {
            if (MenuObject == null)
                Init();

            ShowMenu(true);
        }

        private static void Init()
        {
            MainMenu = uGUI_MainMenu.main ?? Object.FindObjectOfType<uGUI_MainMenu>();
            PanelParent = MainMenu.gameObject.GetComponentInChildren<MainMenuPrimaryOptionsMenu>();


            MenuObject = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SGMMenu"));
            MenuObject.transform.SetParent(PanelParent.transform.parent, false);
            MenuObject.AddComponent<SGMMenuController>();
            //var menuComponent = menuObject.AddComponent<ModManagerMenu>();
            var btnCloseMenu = MenuObject.FindComponentByName<Button>("btnCloseMenu");
            btnCloseMenu.onClick.AddListener(() => ShowMenu(false));
        }

        private static void ShowMenu(bool value)
        {
            // Show it
            MenuObject.SetActive(value);
            // Hide main
            SetMainMenuShown(!value);
        }

        private static void SetMainMenuShown(bool shown)
        {
            if (MainMenu == null)
                return;

            PanelParent.gameObject.SetActive(shown);
        }
    }
}
