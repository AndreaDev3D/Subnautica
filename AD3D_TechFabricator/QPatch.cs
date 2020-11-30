using AD3D.BO.Base;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;

namespace AD3D
{
    [QModCore]
    public class QPatch
    {

        [QModPatch]
        public static void Patch()
        {
            var techFabricator = new AD3D_TechFabricator();
            // add Tabs
            techFabricator.AddTabNode("EnergySolutionID", "Energy Solution", AD3D_DeepEngineMod.BO.Patch.DeepEngine.DeepEngine.GetItemIcon()); // add some tabs
            techFabricator.AddCraftNode("DeepEngine_Kit", "EnergySolutionID");
            techFabricator.Patch();

            AD3D.Helper.LogEvent($"Patched successfully [v{Constant.TechFabricator_Version}]");
        }
    }
}