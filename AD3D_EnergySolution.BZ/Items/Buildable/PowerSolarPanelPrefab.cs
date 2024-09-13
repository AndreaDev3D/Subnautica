using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;
using Nautilus.Extensions;
using Nautilus.Utility;
using System.Collections;
using UnityEngine;

namespace AD3D_EnergySolution.BZ.Items.Buildable
{
    // https://github.com/SubnauticaModding/Subnautica.Templates/blob/main/Subnautica.Templates/templates/SNModding.NautilusTemp/Items/Equipment/YeetKnifePrefab.cs#L25
    // https://github.com/Indigocoder1/Indigocoder_SubnauticaMods/blob/master/Chameleon/Prefabs/Chameleon_Craftable.cs
    public static class PowerSolarPanelPrefab
    {
        public const string _ClassID = "PowerSolarPanel";
        public const string _FriendlyName = "Power Solar Panel";
        public const string _Description = "A Powerful Solar Panel that makes me go yes.";

        public static PrefabInfo PrefabInfo { get; } = PrefabInfo
            .WithTechType(_ClassID, _FriendlyName, _Description, unlockAtStart: true)
            .WithIcon(ImageUtils.LoadSpriteFromTexture(Plugin.AssetsBundle.LoadAsset<Texture2D>($"{_ClassID}.png")));

        public static void Register()
        {
            var customPrefab = new CustomPrefab(PrefabInfo);
            customPrefab.SetGameObject(GetAssetBundlePrefab);

            var recipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new Ingredient(TechType.Titanium, 1),
                    new Ingredient(TechType.Quartz, 2),
                },
            };

            customPrefab.SetRecipe(recipe)
                .WithFabricatorType(CraftTree.Type.Constructor)
                .WithStepsToFabricatorTab("Exterior Modules");

            customPrefab.SetEquipment(EquipmentType.Hand);
            customPrefab.SetPdaGroupCategory(TechGroup.ExteriorModules, TechCategory.ExteriorModule);

            customPrefab.Register();
        }

        private static IEnumerator GetAssetBundlePrefab(IOut<GameObject> clonePrefab)
        {
            GameObject model = Plugin.AssetsBundle.LoadAsset<GameObject>($"{_ClassID}.prefab");
            GameObject prefab = GameObject.Instantiate(model);
            prefab.SetActive(false);

            PrefabUtils.AddBasicComponents(prefab, PrefabInfo.ClassID, PrefabInfo.TechType, LargeWorldEntity.CellLevel.Global);

            MaterialUtils.ApplySNShaders(prefab);

            SetupConstructable(prefab);

            // Clone solar panel
            CoroutineTask<GameObject> task = CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel, true);
            yield return task;
            GameObject solarPanelPrefab = task.GetResult();

            SetupAdditionalComponents(prefab, solarPanelPrefab);

            prefab.SetActive(true);

            clonePrefab.Set(prefab);
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

        private static void SetupAdditionalComponents(GameObject prefab, GameObject clonePrefab)
        {
            // Add components necessary for power management
            prefab.AddComponent<PowerSource>().CopyComponent(clonePrefab.GetComponent<PowerSource>());
            prefab.AddComponent<PowerFX>().CopyComponent(clonePrefab.GetComponent<PowerFX>());
            prefab.AddComponent<PowerRelay>().CopyComponent(clonePrefab.GetComponent<PowerRelay>());
            //prefab.AddComponent<PowerRelay>();
            //prefab.AddComponent<ConstructableBounds>();
            //prefab.AddComponent<HighlightingBlocker>();

            // Custom components related to Deep Engine
            //var deepEngineController = prefab.AddComponent<DeepEngineController>();
            //deepEngineController.MaxPowerAllowed = Plugin.DeepEngineConfig.MaxPowerAllowed;
            //deepEngineController.PowerMultiplier = Plugin.DeepEngineConfig.PowerMultiplier;
            //deepEngineController.MakesNoise = Plugin.DeepEngineConfig.MakesNoise;
            //deepEngineController.Engine_SFX = Plugin.AssetsBundle.LoadAsset<AudioClip>("Engine_FX");
            //deepEngineController.DrillingAnimation = Plugin.AssetsBundle.LoadAsset<AnimationClip>("Drilling.anim");
        }
    }
}
