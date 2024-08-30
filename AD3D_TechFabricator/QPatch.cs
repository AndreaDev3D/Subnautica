using AD3D_TechFabricatorMod.BO.Base;
using AD3D_Common;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;

namespace AD3D_TechFabricatorMod
{
    [QModCore]
    public class QPatch
    {
        //public const string _AssetBundleName = "lightsolution.asset";

        //private static AssetBundle _bundle;
        //public static AssetBundle Bundle => _bundle ?? (_bundle = AD3D_Common.Helper.GetAssetBundle(Assembly.GetCallingAssembly(), _AssetBundleName));

        public const string _ModName = "AD3D_TechFabricatorMod";
        public const string _Mod_Version = "1.0.0";

        [QModPatch]
        public static void Patch()
        {
            var techFabricator = new AD3D_TechFabricator();
            // add Tabs
            var energySolutionTab = "EnergySolutionID";
            techFabricator.AddTabNode(energySolutionTab, "Energy Solution", AD3D_DeepEngineMod.BO.Patch.DeepEngine.DeepEngine.GetItemIcon()); // add EnergySolutionID tab
            techFabricator.AddCraftNode("DeepEngine_Kit", energySolutionTab);

            var foodSolutionId = "FoodSolutionId";
            techFabricator.AddTabNode(foodSolutionId, "Food Solution", Helper.GetSprite("AD3D_TechFabricatorMod", "FoodIcon"));
            techFabricator.AddCraftNode("Bar1", foodSolutionId);

            techFabricator.Patch();

            AD3D_Common.Helper.Log($"{_ModName} Patched successfully [{_Mod_Version}]");
        }
    }
}