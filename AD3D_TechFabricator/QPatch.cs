﻿using AD3D_TechFabricatorMod.BO.Base;
using AD3D_Common;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;

namespace AD3D_TechFabricatorMod
{
    [QModCore]
    public class QPatch
    {

        [QModPatch]
        public static void Patch()
        {
            var techFabricator = new AD3D_TechFabricator();
            // add Tabs
            var energySolutionTab = "EnergySolutionID";
            techFabricator.AddTabNode(energySolutionTab, "Energy Solution", AD3D_DeepEngineMod.BO.Patch.DeepEngine.DeepEngine.GetItemIcon()); // add EnergySolutionID tab
            techFabricator.AddCraftNode("DeepEngine_Kit", energySolutionTab);

            techFabricator.Patch();

            Helper.Log($"Patched successfully [v1.0.0]");
        }
    }
}