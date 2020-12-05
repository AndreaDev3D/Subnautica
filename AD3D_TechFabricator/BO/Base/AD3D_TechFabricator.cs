using System;
using AD3D;
using AD3D_Common;
using DevExpress.Xpo;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Utility;
using UnityEngine;
using UWE;

namespace AD3D.BO.Base
{
    public class AD3D_TechFabricator : CustomFabricator
    {
        public const string _classId = "AD3DTechFabricator";
        public const string _friendlyName = "AD3D TechFabricator";
        public const string _description = "A specialized fabricator for AD3D Technology.";

        public override Models Model => Models.Custom; // Using Custom so the texture can be altered

        internal AD3D_TechFabricator(): base(_classId, _friendlyName, _description)
        {
            ClassID = _classId;
            FriendlyName = _friendlyName;
            Description = _description;
        }

        public override WorldEntityInfo EntityInfo => new WorldEntityInfo() { cellLevel = LargeWorldEntity.CellLevel.Global, classId = this.ClassID, localScale = Vector3.one, prefabZUp = false, slotType = EntitySlot.Type.Small, techType = this.TechType };
        public override TechGroup GroupForPDA => TechGroup.InteriorModules;
        public override TechCategory CategoryForPDA => TechCategory.InteriorModule;
        public override TechType RequiredForUnlock => TechType.ComputerChip;

        // In honor of PrimeSonic I will use the same recipe
        protected override TechData GetBlueprintRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 2),
                    new Ingredient(TechType.ComputerChip, 1),
                    new Ingredient(TechType.Magnetite, 1),
                    new Ingredient(TechType.Lead, 2),
                }
            };
        }
        protected override GameObject GetCustomCrafterPreFab()
        {
            // Instantiate Fabricator object
            var gObj = GameObject.Instantiate(CraftData.GetPrefabForTechType(TechType.Fabricator));

            var customRenderTexture = Helper.GetTexture(Constant.TechFabricator_ModName, "TechFabricator_Texture");
            // Set the custom texture
            if (customRenderTexture != null)
            {
                SkinnedMeshRenderer skinnedMeshRenderer = gObj.GetComponentInChildren<SkinnedMeshRenderer>();
                skinnedMeshRenderer.material.mainTexture = customRenderTexture;
            }

            //// Change size
            //Vector3 scale = gObj.transform.localScale;
            //const float factor = 1.50f;
            //gObj.transform.localScale = new Vector3(scale.x * factor, scale.y * factor, scale.z * factor);

            return gObj;
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return Helper.GetSprite(Constant.TechFabricator_ModName, "TechFabricator_Icon");
        }
    }

}