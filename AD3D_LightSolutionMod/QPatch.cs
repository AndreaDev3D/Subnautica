using AD3D_LightSolutionMod.BO.Base;
using AD3D_LightSolutionMod.BO.Config;
using QModManager.API.ModLoading;

namespace AD3D_LightSolutionMod
{
    [QModCore]
    public class QPatch
    {
        internal static LightSolutionConfig Config { get; set; } = new LightSolutionConfig();
        internal static DatabaseConfig Database { get; set; } = new DatabaseConfig();

        internal static LightSwitch LightSwitch { get; } = new LightSwitch();
        internal static LightSource_1 LightSource_1 { get; } = new LightSource_1();

        [QModPatch]
        public static void Patch()
        {
            Config.Load();
            Database.Load();

            LightSwitch.Patch();
            LightSource_1.Patch();

            //CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, DeepEngineKit.TechType, "Energy Solution");

            //Add the databank entry.
            //LanguageHandler.SetLanguageLine($"Ency_{DeepEngine.ClassID}", DeepEngine.FriendlyName);
            //LanguageHandler.SetLanguageLine($"EncyDesc_{DeepEngine.ClassID}", DeepEngine.PDADescription(Config.MaxPowerAllowed));

            AD3D_Common.Helper.Log($"Patched successfully [v1.0.0]");
        }
    }
}