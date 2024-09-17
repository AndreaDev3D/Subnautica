using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Extensions;
using Nautilus.Utility;
using UnityEngine;

#if SN
using static CraftData;
#endif

namespace AD3D_TransportSolution.Items.Drivable
{
    public class RailItem
    {
        public PrefabInfo PrefabInfo { get; }

        public RailItem(string classID, string friendlyName, string shortDescription)
        {
            PrefabInfo = PrefabInfo
            .WithTechType(classID, friendlyName, shortDescription, unlockAtStart: true)
            .WithIcon(ImageUtils.LoadSpriteFromTexture(Plugin.AssetBundle.LoadAsset<Texture2D>($"{classID}.png")));
        }

        public void Register()
        {
            var customPrefab = new CustomPrefab(PrefabInfo);

            customPrefab.SetGameObject(GetAssetBundlePrefab());

            var recipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 1),
                },
            };

            customPrefab.SetRecipe(recipe)
                .WithFabricatorType(CraftTree.Type.Constructor)
                .WithStepsToFabricatorTab("Exterior Modules");

            customPrefab.SetEquipment(EquipmentType.Hand);
            customPrefab.SetPdaGroupCategory(TechGroup.ExteriorModules, TechCategory.ExteriorModule);

            customPrefab.Register();
        }

        private GameObject GetAssetBundlePrefab()
        {
            var prefab = Plugin.AssetBundle.LoadAsset<GameObject>($"{PrefabInfo.ClassID}.prefab");
            PrefabUtils.AddBasicComponents(prefab, PrefabInfo.ClassID, PrefabInfo.TechType, LargeWorldEntity.CellLevel.Far);
            MaterialUtils.ApplySNShaders(prefab);

            SetupConstructable(prefab);

            return prefab;
        }

        private void SetupConstructable(GameObject prefab)
        {
            var rootModel = prefab.SearchChild("model");
            var constructable = PrefabUtils.AddConstructable(prefab, PrefabInfo.TechType, ConstructableFlags.Outside, rootModel);
            constructable.allowedOnConstructables = true;
            constructable.allowedOutside = true;
            constructable.allowedOnGround = true;
            constructable.forceUpright = true;
            constructable.rotationEnabled = true;
            constructable.placeDefaultDistance = 7.5f;
            constructable.placeMinDistance = 5f;
            constructable.placeMaxDistance = 15f;
        }
    }
}
