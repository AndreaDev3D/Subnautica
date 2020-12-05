using AD3D_DeepEngineMod.BO;
using AD3D_DeepEngineMod.BO.Patch.DeepEngine;
using AD3D_DeepEngineMod.BO.Utils;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;

namespace AD3D_DeepEngineMod
{
    [QModCore]
    public class QPatch
    {
        //public static TechType DeepEngineKit;
        internal static DeepEngineConfig Config { get; set; } = OptionsPanelHandler.Main.RegisterModOptions<DeepEngineConfig>();
        internal static DeepEngineKit DeepEngineKit { get; } = new DeepEngineKit();
        internal static DeepEngine DeepEngine { get; } = new DeepEngine();

        [QModPatch]
        public static void Patch()
        {
            Helper.LoadConfig();

            DeepEngineKit.Patch();
            DeepEngine.Patch();

            //CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, DeepEngineKit.TechType, "Energy Solution");

            //Add the databank entry.
            LanguageHandler.SetLanguageLine($"Ency_{Constant.ClassID}", Constant.FriendlyName);
            LanguageHandler.SetLanguageLine($"EncyDesc_{Constant.ClassID}", Constant.PDADescription(Helper.Config.MaxPowerAllowed));

            AD3D_Common.Helper.Log($"Patched successfully [v{Constant.DeepEngineMod_Version}]");
        }
    }
}