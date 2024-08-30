using AD3D_LightSolution.Items.Buildable;
using AD3D_LightSolutionMod.BO.Config;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Nautilus.Handlers;
using Nautilus.Utility;
using System.Reflection;
using UnityEngine;
using static AD3D_LightSolution.Base.Enumerators;

namespace AD3D_LightSolution
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.snmodding.nautilus")]
    public class Plugin : BaseUnityPlugin
    {
        public new static ManualLogSource Logger { get; private set; }

        private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

        public static AssetBundle AssetBundle { get; private set; }
        public static DatabaseConfig Database { get; private set; }

        private void Awake()
        {
            AssetBundle = AssetBundleLoadingUtils.LoadFromAssetsFolder(Assembly, "lightsolution.asset");

            Database = OptionsPanelHandler.RegisterModOptions<DatabaseConfig>();
            Database.Load();

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
            new LightSwitchPrefab("LightSwitch", "Light Switch", "The light switch provide a versatile solution to handle in once place a set of \"Light Source\" items.").Register();

            new LightSourcePrefab("LightSource_1", "LightSource 1", LightSourceItemType.Roof).Register();
            new LightSourcePrefab("LightSource_2", "LightSource 2", LightSourceItemType.Roof).Register();
            new LightSourcePrefab("LightSource_3", "LightSource 3", LightSourceItemType.Wall).Register();
            new LightSourcePrefab("LightSource_4", "LightSource 4", LightSourceItemType.Wall).Register();
            new LightSourcePrefab("LightSource_5", "LightSource 5", LightSourceItemType.Floor).Register();
            new LightSourcePrefab("LightSource_6", "LightSource 6", LightSourceItemType.Floor).Register();
            new LightSourcePrefab("LightSource_7", "LightSource 7", LightSourceItemType.Floor).Register();
            new LightSourcePrefab("LightSource_8", "LightSource 8", LightSourceItemType.Wall).Register();


            new LightStandPrefab("LightStand_1", "LightStand 1").Register();
            new LightStandPrefab("LightStand_2", "LightStand 2").Register();
        }
    }
}