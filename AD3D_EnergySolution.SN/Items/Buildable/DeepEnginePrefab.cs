using AD3D_EnergySolution.SN.BO.Runtime;
using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Crafting;
using Nautilus.Utility;
using UnityEngine;

namespace AD3D_EnergySolution.SN.Items.Buildable
{
    public static class DeepEnginePrefab
    {
        public const string _ClassID = "DeepEngine_v2";
        public const string _FriendlyName = "Deep Engine v2";
        public const string _Description = "High efficiency electric generator that runs in deep water.";
        
        public static PrefabInfo PrefabInfo { get; } = PrefabInfo
            .WithTechType(_ClassID, _FriendlyName, _Description, unlockAtStart: true)
            .WithIcon(ImageUtils.LoadSpriteFromTexture(Plugin.AssetsBundle.LoadAsset<Texture2D>("DeepEngine.png")))
            .WithSizeInInventory(new Vector2int(2, 2));


        public static void Register()
        {
            var customPrefab = new CustomPrefab(PrefabInfo);

            customPrefab.SetGameObject(GetAssetBundlePrefab());

            var recipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new CraftData.Ingredient(TechType.Titanium, 2),
                    new CraftData.Ingredient(TechType.WiringKit, 1),
                },
            };

            customPrefab.SetRecipe(recipe)
                .WithFabricatorType(CraftTree.Type.Constructor)
                .WithStepsToFabricatorTab("Exterior Modules");

            customPrefab.SetEquipment(EquipmentType.Hand);
            customPrefab.SetPdaGroupCategory(TechGroup.ExteriorModules, TechCategory.ExteriorModule);

            customPrefab.Register();
        }

        private static GameObject GetAssetBundlePrefab()
        {
            GameObject prefab = Plugin.AssetsBundle.LoadAsset<GameObject>("DeepEngine.prefab");

            prefab.SetActive(false);

            PrefabUtils.AddBasicComponents(prefab, PrefabInfo.ClassID, PrefabInfo.TechType, LargeWorldEntity.CellLevel.Global);

            MaterialUtils.ApplySNShaders(prefab);

            SetupConstructable(prefab);

            SetupAdditionalComponents(prefab);

            prefab.SetActive(true);

            return prefab;
        }

        private static void SetupConstructable(GameObject prefab)
        {
            var constructible = PrefabUtils.AddConstructable(prefab, PrefabInfo.TechType, ConstructableFlags.Outside, prefab.transform.GetChild(0).gameObject);

            constructible.constructedAmount = 1;
            constructible.allowedInBase = false;
            constructible.allowedInSub = false;
            constructible.allowedOutside = true;
            constructible.allowedOnCeiling = false;
            constructible.allowedOnGround = true;
            constructible.allowedOnWall = false;
            constructible.allowedOnConstructables = false;
            constructible.rotationEnabled = true;
            constructible.placeDefaultDistance = 6f;
            constructible.placeMinDistance = 0.5f;
            constructible.placeMaxDistance = 15f;
            constructible.surfaceType = VFXSurfaceTypes.metal;
            constructible.forceUpright = true;
        }
        private static void SetupAdditionalComponents(GameObject prefab)
        {
            // Add components necessary for power management
            //prefab.AddComponent<PowerSource>();
            //prefab.AddComponent<PowerFX>();
            //prefab.AddComponent<PowerRelay>();
            //prefab.AddComponent<ConstructableBounds>();
            //prefab.AddComponent<HighlightingBlocker>();

            // Custom components related to Deep Engine
            var deepEngineController = prefab.AddComponent<DeepEngineController>();
            deepEngineController.MaxPowerAllowed = Plugin.DeepEngineConfig.MaxPowerAllowed;
            deepEngineController.PowerMultiplier = Plugin.DeepEngineConfig.PowerMultiplier;
            deepEngineController.MakesNoise = Plugin.DeepEngineConfig.MakesNoise;
            deepEngineController.Engine_SFX = Plugin.AssetsBundle.LoadAsset<AudioClip>("Engine_FX");
            deepEngineController.DrillingAnimation = Plugin.AssetsBundle.LoadAsset<AnimationClip>("Drilling.anim");
        }
    }
}
