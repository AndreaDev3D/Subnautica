using AD3D_DeepEngineMod.BO;
using AD3D_DeepEngineMod.BO.Helper;
using AD3D_DeepEngineMod.BO.Patch;
using AD3D_DeepEngineMod.BO.Patch.DeepEngine;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;
using System.Collections.Generic;

namespace AD3D_DeepEngineMod
{
    [QModCore]
    public class QPatch
    {
        //public static TechType DeepEngineKit;
        //internal static DeepEngineConfig Config { get; } = OptionsPanelHandler.Main.RegisterModOptions<DeepEngineConfig>();
        internal static DeepEngineKit DeepEngineKit { get; } = new DeepEngineKit();
        internal static DeepEngine DeepEngine { get; } = new DeepEngine();

        [QModPatch]
        public static void Patch()
        {
            Import.LoadConfig();

            //patch crafting recipes
            //DeepEngineKit = TechTypeHandler.AddTechType($"{Constant.ClassID}_Kit", Constant.FriendlyName, Constant.ShortDescription, Helper.GetSpriteFromBundle("Icon"));
            //CraftDataHandler.SetItemSize(DeepEngineKit, new Vector2int(2, 2));
            //var techDataBlade = new TechData()
            //{
            //    craftAmount = 1,
            //    Ingredients = new List<Ingredient>()
            //    {
            //      new Ingredient(TechType.Titanium, 2),
            //      new Ingredient(TechType.Lubricant, 1),
            //      new Ingredient(TechType.WiringKit, 1),
            //    }
            //};
            //CraftDataHandler.SetTechData(DeepEngineKit, techDataBlade); 

            DeepEngineKit.Patch();
            DeepEngine.Patch();

            CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, DeepEngineKit.TechType, "Energy Solution");
            //CraftTreeHandler.AddCraftingNode(CraftTree.Type.Fabricator, deepEngineKit, "Energy Solution");


            //Add the databank entry.
            LanguageHandler.SetLanguageLine($"Ency_{Constant.ClassID}", Constant.FriendlyName);
            LanguageHandler.SetLanguageLine($"EncyDesc_{Constant.ClassID}", Constant.PDADescription(Import.Config.MaxPowerAllowed));

            Logger.Log(Logger.Level.Info, "Patched successfully!");
        }
    }
}