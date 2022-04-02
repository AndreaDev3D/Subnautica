using HarmonyLib;
using QModManager.API.ModLoading;

namespace AD3D_VeichlePackMod
{
    [QModCore]
    public static class QPatch
    {
        [QModPatch]
        public static void Patch()
        {
            var krakenObj = new Kraken();

            new Harmony("com.AndreaDev3D.subnautica.AD3D_VeichlePackMod.mod").PatchAll();
            Kraken.Register();
        }
    }
}
