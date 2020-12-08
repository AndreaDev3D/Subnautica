using AD3D_LightSolutionMod.BO.Config;
using AD3D_LightSolutionMod.BO.Patch.DeepEngine;
using AD3D_LightSolutionMod.BO.Utils;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;

namespace AD3D_LightSolutionMod
{
    [QModCore]
    public class QPatch
    {
        //public static TechType DeepEngineKit;
        internal static DeepEngineConfig Config { get; set; } = new DeepEngineConfig();
        internal static DeepEngineKit DeepEngineKit { get; } = new DeepEngineKit();
        internal static DeepEngine DeepEngine { get; } = new DeepEngine();

        [QModPatch]
        public static void Patch()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<DeepEngineConfig>();
            Config.Load();

            DeepEngineKit.Patch();
            DeepEngine.Patch();

            //CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, DeepEngineKit.TechType, "Energy Solution");

            //Add the databank entry.
            LanguageHandler.SetLanguageLine($"Ency_{DeepEngine.ClassID}", DeepEngine.FriendlyName);
            LanguageHandler.SetLanguageLine($"EncyDesc_{DeepEngine.ClassID}", DeepEngine.PDADescription(Config.MaxPowerAllowed));

            AD3D_Common.Helper.Log($"Patched successfully [v1.3.0]");
        }
    }
}