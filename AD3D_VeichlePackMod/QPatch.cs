using HarmonyLib;
using QModManager.API.ModLoading;
using System.Reflection;
using UnityEngine;

namespace AD3D_VeichlePackMod
{
    [QModCore]
    public static class QPatch
    {
        public const string _AssetBundleName = "veichlepack.asset";
        public new static string _AssemblyLocation { get; } = Assembly.GetCallingAssembly().Location;

        private static AssetBundle _bundle;
        public static AssetBundle Bundle => _bundle ??= AD3D_Common.Helper.GetAssetBundle(Assembly.GetCallingAssembly().Location, _AssetBundleName);

        public const string _ModName = "AD3D_VeichlePackMod";
        public const string _Mod_Version = "1.0.0";

        [QModPatch]
        public static void Patch()
        {
            var krakenObj = new Kraken();

            new Harmony("com.AndreaDev3D.subnautica.AD3D_VeichlePackMod.mod").PatchAll();
            Kraken.Register();


            AD3D_Common.Helper.Log($"{_ModName} Patched successfully [{_Mod_Version}]");
        }
    }
}
