using AD3D_Common;
using System;
using UnityEngine;
using UWE;

namespace AD3D_HabitatSolutionMod.BO.InGame
{
    public class HabitatBubble : MonoBehaviour
    {
        bool IsInside = false;
        public void Start()
        {
            try
            {
                var _prefab = this.gameObject;
                var seamothRef = CraftData.GetPrefabForTechType(TechType.Seamoth);
                // SeaMoth
                //var seaMoth = _prefab.AddComponent<SeaMoth>();
                //seaMoth.handLabel = "Enter Habitat";

                //var BatterySlot = GameObjectFinder.FindByName(_prefab, "BatterySlot");
                //BatterySlot.AddComponent<ChildObjectIdentifier>();
                //var BatteryInput = GameObjectFinder.FindByName(_prefab, "BatteryInput");
                // GenericHandTarget
                //var genericHandTarget = BatteryInput.AddComponent<GenericHandTarget>();
                // OxygenManager
                var oxygenManager = _prefab.AddComponent<OxygenManager>();
                // DealDamageOnImpact
//                _prefab.AddComponent<DealDamageOnImpact>();
//                // LiveMixin
//                var liveMixin = _prefab.AddComponent<LiveMixin>();
//                liveMixin.data = ScriptableObject.CreateInstance<LiveMixinData>();
//                liveMixin.data.maxHealth = 300;
//                liveMixin = seamothRef.GetComponent<LiveMixin>();
//                //seaMoth.liveMixin = liveMixin;
//                // VFXConstructing
//                var vFXConstructing = _prefab.AddComponent<VFXConstructing>();
//                vFXConstructing = seamothRef.GetComponent<VFXConstructing>();
//                // FMOD_StudioEventEmitter
//                _prefab.AddComponent<FMOD_StudioEventEmitter>();
//                var eMx = _prefab.AddComponent<EnergyMixin>();
//                eMx = seamothRef.GetComponent<EnergyMixin>();
//                //eMx.storageRoot = BatterySlot.GetComponent<ChildObjectIdentifier>();
//                eMx.compatibleBatteries.Add(TechType.PowerCell);
//                eMx.compatibleBatteries.Add(TechType.PrecursorIonBattery);
//                eMx.allowBatteryReplacement = true;
//                //eMx.batteryModels.ToList().Add(new GameObject(new GameObject(), TechType.PowerCell)));

//                //genericHandTarget.onHandHover.AddListener(eMx.HandHover);
//                // WorldForces
//                var WorldForces = _prefab.AddComponent<WorldForces>();
//                WorldForces.useRigidbody = _prefab.GetComponent<Rigidbody>();
//                // GameInfoIcon
//                var gameInfoIcon = _prefab.AddComponent<GameInfoIcon>();
//                gameInfoIcon.techType = TechType.Seamoth;
//                // CrushDamage
//                var crushDamage = _prefab.AddComponent<CrushDamage>();
//                crushDamage.liveMixin = liveMixin;
//                //crushDamage.vehicle = seaMoth;
//#warning add CrushDepth and Notification
//                // ConditionRules
//                var conditionRules = _prefab.AddComponent<ConditionRules>();
//                // DepthAlarms
//                var depthAlarms = _prefab.AddComponent<DepthAlarms>();
//                // FMOD_CustomEmitter
//                var fMOD_CustomEmitter = _prefab.AddComponent<FMOD_CustomEmitter>();
//                fMOD_CustomEmitter = seamothRef.GetComponent<FMOD_CustomEmitter>();
//                // FMOD_StudioEventEmitter
//                _prefab.AddComponent<FMOD_StudioEventEmitter>();
//                // EnergyEffect
//                var energyEffect = _prefab.AddComponent<EnergyEffect>();


                //Spawn a seamoth for reference.
                //var seamothRef = CraftData.GetPrefabForTechType(TechType.Cyclops);
                //Get the seamoth's water clip proxy component. This is what displaces the water.
                var seamothProxy = seamothRef.GetComponentInChildren<WaterClipProxy>();
                //Find the parent of all the ship's clip proxys.
                Transform proxyParent = GameObjectFinder.FindByName(this.gameObject, "Habitat_Proxy").transform;
                //Loop through them all

                //this.depthClearer = proxyParent.gameObject.GetComponent<Renderer>();
                var waterClip = proxyParent.gameObject.AddComponent<WaterClipProxy>();
                waterClip.shape = WaterClipProxy.Shape.Box;
                waterClip.clipMaterial = seamothProxy.clipMaterial;
                waterClip.gameObject.layer = 28;
                //Unload the prefab to save on resources.
                //Resources.UnloadAsset(seamothRef);
            }
            catch (Exception ex) 
            {
                Helper.Log($"ERROR : {ex.Message} [{ex.StackTrace}]", true); 
            }
        }

        private void FixedUpdate()
        {
            if (IsInside)
            {
                Player.main.SetMotorMode(Player.MotorMode.Walk);
                return;
            }

            Player.main.SetMotorMode(Player.MotorMode.Dive);
        }

        private void Update()
        {
            if(IsInside)
                Player.main.SetMotorMode(Player.MotorMode.Walk);

            Player.main.SetMotorMode(Player.MotorMode.Dive);
        }

        private void LateUpdate()
        {
            if (IsInside)
                Player.main.SetMotorMode(Player.MotorMode.Walk);

            Player.main.SetMotorMode(Player.MotorMode.Dive);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == Player.main.tag) 
            {
                Helper.Log("PlayerIn", showOnScreen: true);
                //Player.main.SetMotorMode(Player.MotorMode.Walk);
                IsInside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Player.main.tag)
            {
                Helper.Log("PlayerOut", showOnScreen: true); IsInside = false;
                //Player.main.SetMotorMode(Player.MotorMode.Dive);
                IsInside = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == Player.main.tag)
            {
                Player.main.oxygenMgr.AddOxygen(1);
            }
        }
    }
}
