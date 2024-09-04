using AD3D_EnergySolution.SN.Config;
using AD3D_EnergySolution.SN.Items.Buildable;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using System.Reflection;
using UnityEngine;

namespace AD3D_EnergySolution.SN
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }
        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
        public static AssetBundle AssetsBundle { get; private set; }
        public static DeepEngineConfig DeepEngineConfig { get; private set; }

        private void Awake()
        {
            AssetsBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "energysolution.asset");

            DeepEngineConfig = OptionsPanelHandler.RegisterModOptions<DeepEngineConfig>();
            DeepEngineConfig.Load();

            // set project-scoped logger instance
            Logger = base.Logger;

            // Initialize custom prefabs
            InitializePrefabs();

            // register harmony patches, if there are any
            Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        private void InitializePrefabs()
        {
            //BetterSolarPanelPrefab.Register();
            DeepEnginePrefab.Register();
        }
    }
}