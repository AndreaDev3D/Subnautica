using System;
using System.Collections.Generic;
using System.Linq;
using AD3D_Common;
using DevExpress.Xpo;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;

namespace AD3D_HabitatSolutionMod.BO.Base
{

    public class StoragebaleItem : Buildable
    {
        public const string _ClassID = "QuantumStorage";
        public const string _FriendlyName = "Quantum Storage";
        public const string _ShortDescription = "Programmable dark matter storage to allocate quantum particle.";
        public StoragebaleItem() : base(_ClassID, _FriendlyName, _ShortDescription)
        {
        }
        public override TechGroup GroupForPDA => TechGroup.InteriorModules;
        public override TechCategory CategoryForPDA => TechCategory.InteriorModule;

        protected override TechData GetBlueprintRecipe()
        {
            return new TechData()
            {
                craftAmount = 1,
                Ingredients = new List<Ingredient>(new Ingredient[2]
                {
                  new Ingredient(TechType.Titanium, 1),
                  new Ingredient(TechType.Lithium, 1),
                })
            };
        }

        public override GameObject GetGameObject()
        {
            //Instantiates a copy of the prefab that is loaded from the AssetBundle loaded above.
            GameObject _prefab = GameObject.Instantiate(QPatch.Bundle.LoadAsset<GameObject>($"{_ClassID}.prefab"));
            _prefab.name = _ClassID;
            //Need a tech tag for most prefabs
            var techTag = _prefab.AddComponent<TechTag>();
            techTag.type = TechType;
            _prefab.EnsureComponent<LargeWorldEntity>().cellLevel = LargeWorldEntity.CellLevel.Global;
            _prefab.EnsureComponent<PrefabIdentifier>().ClassId = ClassID;
            //Update all shaders
            ApplySubnauticaShaders(_prefab, true);
            ApplySubnauticaSky(_prefab);

            // Add constructable - This prefab normally isn't constructed.
            var rootModel = GameObjectFinder.FindByName(_prefab, "model");
            Constructable constructible = _prefab.AddComponent<Constructable>();
            constructible.constructedAmount = 1;
            constructible.techType = this.TechType;
            constructible.model = rootModel;
            //constructible.builtBoxFX = null;
            constructible.controlModelState = true;
            constructible.allowedOnWall = true;
            constructible.allowedOnGround = false;
            constructible.allowedOnCeiling = false;
            constructible.deconstructionAllowed = true;
            constructible.allowedInSub = true;
            constructible.allowedInBase = true;
            constructible.allowedOutside = true;
            constructible.allowedOnConstructables = true;
            constructible.forceUpright = true;
            constructible.rotationEnabled = false;
            constructible.placeDefaultDistance = 2f;
            constructible.placeMinDistance = 1.2f;
            constructible.placeMaxDistance = 5f;
            constructible.surfaceType = VFXSurfaceTypes.metal;


            _prefab.SetActive(false);

            var StorageRoot = GameObjectFinder.FindByName(_prefab, "StorageRoot");
            var StorageRootChild = StorageRoot.EnsureComponent<ChildObjectIdentifier>();
            StorageRootChild.ClassId = $"{ClassID}Container";

            var storage = _prefab.AddComponent<StorageContainer>();
            storage.prefabRoot = _prefab;
            storage.width = 8;
            storage.height = 9;
            storage.hoverText = $"Open {ClassID}";
            storage.modelSizeRadius = 1;
            storage.storageLabel = $"{ClassID}";
            storage.storageRoot = StorageRootChild;
            storage.preventDeconstructionIfNotEmpty = true;

            _prefab.SetActive(true);
            return _prefab;
        }

        private static void ApplySubnauticaShaders(GameObject gameObject, bool hasLight)
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
                    if (hasLight)
                    {
                        //These enable the item to emit a glow of its own using Subnauticas shader system.
                        material.EnableKeyword("MARMO_EMISSION");
                        material.SetFloat(ShaderPropertyID._EnableGlow, 1f);
                        material.SetTexture(ShaderPropertyID._Illum, emissionTexture);
                        material.SetColor(ShaderPropertyID._GlowColor, new Color(1f, 1f, 1f, 1f));

                        material.SetFloat(ShaderPropertyID._GlowStrength, 5f);
                        material.SetFloat(ShaderPropertyID._GlowStrengthNight, 5f);
                    }
                    if (material.name.Contains("Transparent") || material.name.Contains("Glass"))
                    {

                        material.EnableKeyword("MARMO_ALPHA_CLIP");
                        material.EnableKeyword("MARMO_SIMPLE_GLASS");
                        material.EnableKeyword("MARMO_SPECMAP");
                        material.EnableKeyword("_ZWRITE_ON");
                        material.EnableKeyword("WBOIT");

                        material.SetFloat("_Fresnel", 1.0f);
                        material.SetFloat("_Shininess", 6.0f);
                        material.SetFloat("_SpecInt", 10.0f);
                        material.SetInt("_ZWrite", 0);
                        material.SetInt("_Cutoff", 0);
                        material.SetFloat("_SrcBlend", 1f);
                        material.SetFloat("_DstBlend", 1f);
                        material.SetFloat("_SrcBlend2", 0f);
                        material.SetFloat("_DstBlend2", 10f);
                        material.SetFloat("_AddSrcBlend", 1f);
                        material.SetFloat("_AddDstBlend", 1f);
                        material.SetFloat("_AddSrcBlend2", 0f);
                        material.SetFloat("_AddDstBlend2", 10f);
                        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack | MaterialGlobalIlluminationFlags.RealtimeEmissive;
                        material.renderQueue = 3101;
                        material.enableInstancing = true;
                    }
                }
            }

        }
        private static void ApplySubnauticaSky(GameObject gameObject)
        {
            List<Renderer> Renderers = gameObject.GetComponentsInChildren<Renderer>().ToList();
            foreach (Renderer renderer in Renderers)
            {
                foreach (Material material in renderer.materials)
                {
                    //This applies the games sky lighting to the object when in the game but also only really works combined with the above code as well.
                    SkyApplier skyApplier = gameObject.EnsureComponent<SkyApplier>();
                    skyApplier.renderers = Renderers.ToArray();
                    skyApplier.anchorSky = Skies.Auto;
                }
            }

        }

        protected override Atlas.Sprite GetItemSprite() => Helper.GetSpriteFromBundle(QPatch.Bundle, $"{_ClassID}");

    }
}