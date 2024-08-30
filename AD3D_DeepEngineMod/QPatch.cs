using AD3D_Common.BO.Base;
using AD3D_DeepEngineMod.BO.Base.SolarSource;
using AD3D_DeepEngineMod.BO.Config;
using AD3D_DeepEngineMod.BO.Patch.DeepEngine;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;
using System.Reflection;
using UnityEngine;

namespace AD3D_DeepEngineMod
{
    [QModCore]
    public class QPatch : QPatchBase
    {
        public new const string _AssetBundleName = "deepengine.asset";

        private static AssetBundle _bundle;
        public static AssetBundle Bundle => _bundle ??= AD3D_Common.Helper.GetAssetBundle(Assembly.GetCallingAssembly().Location, _AssetBundleName);


        public const string _ModName = "AD3D_DeepEngineMod";
        public const string _Mod_Version = "1.0.0";

        internal static DeepEngineConfig Config { get; set; } = new DeepEngineConfig();
        internal static DeepEngineKit DeepEngineKit { get; } = new DeepEngineKit();
        internal static DeepEngine DeepEngine { get; } = new DeepEngine();
        internal static SolarPanelItem SolarPanelItem { get; } = new SolarPanelItem();

        [QModPatch]
        public static void Patch()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<DeepEngineConfig>();
            Config.Load();

            DeepEngineKit.Patch();
            DeepEngine.Patch();



            SolarPanelItem.Patch();

            AD3D_Common.Helper.Log($"{_ModName} Patched successfully [{_Mod_Version}]");
        }

        [QModPostPatch]
        public static void PostPatch()
        {
            //Add the databank entry.
            LanguageHandler.SetLanguageLine($"Ency_{DeepEngine.ClassID}", DeepEngine.FriendlyName);
            LanguageHandler.SetLanguageLine($"EncyDesc_{DeepEngine.ClassID}", DeepEngine.PDADescription(Config.MaxPowerAllowed));
        }
    }
}