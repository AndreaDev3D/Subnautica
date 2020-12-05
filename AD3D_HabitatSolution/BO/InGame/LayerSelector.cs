using AD3D_Common;
using System;
using UnityEngine;

namespace AD3D_HabitatSolution.BO.InGame
{
    public class LayerSelector : SubRoot
    {
        public override void Awake()
        {
            try
            {
                this.isBase = true;
                modulesRoot = this.transform;
                lightControl = GameObjectFinder.FindByName(this.gameObject, "LightingController").AddComponent<LightingController>();

                voiceNotificationManager = GameObjectFinder.FindByName(this.gameObject, "VoiceSource").AddComponent<VoiceNotificationManager>();
                voiceNotificationManager.subRoot = this;
                //base.Awake();


                //Spawn a seamoth for reference.
                var seamothRef = CraftData.GetPrefabForTechType(TechType.Seamoth);
                //Get the seamoth's water clip proxy component. This is what displaces the water.
                var seamothProxy = seamothRef.GetComponentInChildren<WaterClipProxy>();
                //Find the parent of all the ship's clip proxys.
                Transform proxyParent = GameObjectFinder.FindByName(this.gameObject, "ProxyParent").transform;
                //Loop through them all
                foreach (Transform child in proxyParent)
                {
                    var waterClip = child.gameObject.AddComponent<WaterClipProxy>();
                    waterClip.shape = WaterClipProxy.Shape.Box;
                    //Apply the seamoth's clip material. No idea what shader it uses or what settings it actually has, so this is an easier option. Reuse the game's assets.
                    waterClip.clipMaterial = seamothProxy.clipMaterial;
                    //You need to do this. By default the layer is 0. This makes it displace everything in the default rendering layer. We only want to displace water.
                    waterClip.gameObject.layer = 28;// SeaMoth layer is 28
                }
                //Unload the prefab to save on resources.
                //Resources.UnloadAsset(seamothRef);
            }
            catch (Exception ex)
            {

                AD3D_HabitatSolution.BO.Utils.Helper.LogEvent($"ERROR : {ex.Message} [{ex.StackTrace}]", true);
            }
        }
    }
}
