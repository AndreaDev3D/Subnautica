using AD3D_Common.Utils;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_SubnauticaGitManager.Items.Menu
{
    public class SGMMenuHandler
    {
        public GameObject MenuObject;// { get; private set; }
        private uGUI_MainMenu MainMenu;// { get; private set; }
        public MainMenuPrimaryOptionsMenu PanelParent;// { get; private set; }
        public SGMMenuController MenuController;

        public void Show()
        {
            if (MenuObject == null)
                Init();

            MenuController?.ShowMenu(true);
        }

        private void Init()
        {
            MainMenu = uGUI_MainMenu.main ?? Object.FindObjectOfType<uGUI_MainMenu>();
            PanelParent = MainMenu.gameObject.GetComponentInChildren<MainMenuPrimaryOptionsMenu>();


            MenuObject = Object.Instantiate(Plugin.AssetBundle.LoadAsset<GameObject>("SGMMenu"));
            MenuObject.transform.SetParent(PanelParent.transform.parent, false);
            MenuController = MenuObject.AddComponent<SGMMenuController>();
            MenuController.MainMenu = MainMenu;
            MenuController.PanelParent = PanelParent;

            //var menuComponent = menuObject.AddComponent<ModManagerMenu>();
            //var btnCloseMenu = MenuObject.FindComponentByName<Button>("btnCloseMenu");
            //btnCloseMenu.onClick.AddListener(() => MenuController.ShowMenu(false));
        }
    }
}
