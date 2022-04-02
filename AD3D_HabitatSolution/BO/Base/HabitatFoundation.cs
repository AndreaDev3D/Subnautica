using AD3D_HabitatSolutionMod.BO.InGame;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UWE;

namespace AD3D_HabitatSolutionMod.BO.Base
{

    public class HabitatFoundation : Buildable
    {
        public const string _ClassID = "HabitatFoundation";
        public const string _FriendlyName = "Habitat Foundation";
        public const string _Description = "This is a buildable Habitat Foundation";
        public HabitatFoundation() : base(_ClassID, _FriendlyName, _Description)
        {
        }
        public override WorldEntityInfo EntityInfo => new WorldEntityInfo() { cellLevel = LargeWorldEntity.CellLevel.Global, classId = this.ClassID, localScale = Vector3.one, prefabZUp = false, slotType = EntitySlot.Type.Small, techType = this.TechType };
        public override TechGroup GroupForPDA => TechGroup.ExteriorModules;
        public override TechCategory CategoryForPDA => TechCategory.ExteriorModule;
        protected override TechData GetBlueprintRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[1]
                {
                  new Ingredient(TechType.Titanium, 2),
                })
            };
        }
        public override GameObject GetGameObject()
        {
            // Instantiates a copy of the prefab that is loaded from the AssetBundle loaded above.
            GameObject _prefab = GameObject.Instantiate(BO.Utils.Helper.Bundle.LoadAsset<GameObject>($"{_ClassID}.prefab"));
            _prefab.name = _ClassID;
            // Need a tech tag for most prefabs
            var techTag = _prefab.AddComponent<TechTag>();
            techTag.type = TechType;

            _prefab.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
            _prefab.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;

            // Update all shaders
            ApplySubnauticaShaders(_prefab);

            // Add constructable
            Constructable constructible = _prefab.AddComponent<Constructable>();
            constructible.constructedAmount = 1;
            constructible.allowedInBase = false;
            constructible.allowedInSub = false;
            constructible.allowedOutside = true;
            constructible.allowedOnCeiling = false;
            constructible.allowedOnGround = true;
            constructible.allowedOnWall = false;
            constructible.allowedOnConstructables = true;
            constructible.techType = this.TechType;
            constructible.rotationEnabled = true;
            constructible.placeDefaultDistance = 20f;
            constructible.placeMinDistance = 10f;
            constructible.placeMaxDistance = 35f;
            constructible.surfaceType = VFXSurfaceTypes.metal;
            constructible.model = AD3D_Common.GameObjectFinder.FindByName(_prefab, "model");//.transform.GetChild(0).gameObject;
            constructible.forceUpright = false;

            _prefab.SetActive(false);
            // Add Custom scripts
            _prefab.AddComponent<HabitatFoundationSystem>();

            _prefab.SetActive(true);
            return _prefab;
        }

        private static void ApplySubnauticaShaders(GameObject gameObject)
        {
            Shader shader = Shader.Find("MarmosetUBER");
            List<Renderer> Renderers = gameObject.GetComponentsInChildren<Renderer>().ToList();

            foreach (Renderer renderer in Renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    //get the old emission before overwriting the shader
                    Texture emissionTexture = material.GetTexture("_EmissionMap");

                    //overwrites your prefabs shader with the shader system from the game.
                    material.shader = shader;

                    //These enable the item to emit a glow of its own using Subnauticas shader system.
                    //material.EnableKeyword("MARMO_EMISSION");
                    material.SetFloat(ShaderPropertyID._EnableGlow, 1f);
                    material.SetTexture(ShaderPropertyID._Illum, emissionTexture);
                    material.SetColor(ShaderPropertyID._GlowColor, new Color(1, 1f, 1, 1));
                }
            }

            //This applies the games sky lighting to the object when in the game but also only really works combined with the above code as well.
            SkyApplier skyApplier = gameObject.EnsureComponent<SkyApplier>();
            skyApplier.renderers = Renderers.ToArray();
            skyApplier.anchorSky = Skies.Auto;
        }

        protected override Atlas.Sprite GetItemSprite()
        {
            return AD3D_Common.Helper.GetSpriteFromBundle(Utils.Helper.Bundle, _ClassID);
        }
    }

}