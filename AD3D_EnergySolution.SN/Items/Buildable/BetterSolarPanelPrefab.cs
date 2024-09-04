using Nautilus.Assets;
using Nautilus.Assets.Gadgets;
using Nautilus.Assets.PrefabTemplates;
using Nautilus.Crafting;

namespace AD3D_EnergySolution.SN.Items.Buildable
{
    public static class BetterSolarPanelPrefab
    {
        public static PrefabInfo Info { get; } = PrefabInfo
            .WithTechType("better_solar_panel", "Better Solar Panel", "A Powerful Solar Panel that makes me go yes.")
            .WithIcon(SpriteManager.Get(TechType.SolarPanel));

        public static void Register()
        {
            var customPrefab = new CustomPrefab(Info);

            var baseObj = new CloneTemplate(Info, TechType.SolarPanel);
            baseObj.ModifyPrefab += obj =>
            {
                var newSolarPanel = obj.GetComponent<SolarPanel>();
                newSolarPanel.maxDepth = 1000f;
                newSolarPanel.powerSource.maxPower = 75 * 3;
            };

            customPrefab.SetGameObject(baseObj);

            var recipe = new RecipeData()
            {
                craftAmount = 1,
                Ingredients =
                {
                    new CraftData.Ingredient(TechType.Titanium, 1),
                    new CraftData.Ingredient(TechType.Quartz, 2),
                },
            };

            customPrefab.SetRecipe(recipe)
                .WithFabricatorType(CraftTree.Type.Constructor)
                .WithStepsToFabricatorTab("Exterior Modules");

            customPrefab.SetEquipment(EquipmentType.Hand);

            customPrefab.SetPdaGroupCategory(TechGroup.ExteriorModules, TechCategory.ExteriorModule);

            customPrefab.Register();
        }
    }
}
