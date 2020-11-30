using AD3D_DeepEngineMod.BO.Helper;
using AD3D_DeepEngineMod.BO.Patch.DeepEngine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_DeepEngineMod.Patch
{
    public class DeepEngineAction : HandTarget, IHandTarget 
    {

        [AssertNotNull]
        public PowerRelay _powerRelay;
        public PowerFX _powerFX;
        public PowerSource _powerSource;

        public AudioClip AudioClip;
        public AudioSource AudioSource;

        public Text lblStatus;
        public Text lblBattery;
        public Text lblEmission;
        public Text lblDepth;

        public float CurrentEmitRate;

        public Constructable Constructable => gameObject.GetComponent<Constructable>();

        private bool _isEnabled => this.GetComponent<DeepEngineAnim>().IsEnabled;

        public void Awake()
        {
            PDAEncyclopedia.Add(Constant.ClassID, true);
        }

        public void Start()
        {
            // Init UI
            lblStatus = GameObjectFinder.FindByName(this.gameObject, "lblStatus").gameObject.GetComponent<Text>();
            lblBattery = GameObjectFinder.FindByName(this.gameObject, "lblBattery").gameObject.GetComponent<Text>();
            lblEmission = GameObjectFinder.FindByName(this.gameObject, "lblEmission").gameObject.GetComponent<Text>();
            lblDepth = GameObjectFinder.FindByName(this.gameObject, "lblDepth").gameObject.GetComponent<Text>();
            // Set Depth
            SetEmittedRate();
            lblDepth.text = $"{Mathf.RoundToInt(Mathf.Abs(this.gameObject.transform.position.y)).ToString()} m";
            //Activate
            PowerRelay solarPowerRelay = CraftData.GetPrefabForTechType(TechType.SolarPanel).GetComponent<PowerRelay>();

            _powerSource = this.gameObject.AddComponent<PowerSource>();
            _powerSource.maxPower = Import.Config.MaxPowerAllowed;

            _powerFX = this.gameObject.AddComponent<PowerFX>();
            _powerFX.vfxPrefab = solarPowerRelay.powerFX.vfxPrefab;
            _powerFX.attachPoint = this.gameObject.transform;

            _powerRelay = this.gameObject.AddComponent<PowerRelay>();
            _powerRelay.powerFX = _powerFX;
            _powerRelay.maxOutboundDistance = 15;
            _powerRelay.internalPowerSource = _powerSource;
            // Play Audio
            SetupAudio();
            //Start the coroutine we define below named ExampleCoroutine.
            StartCoroutine(EmitEnergy());
            StartCoroutine(UpdateUI());

            Import.LogEvent("DeepEngineAction Activate");
        }

        IEnumerator EmitEnergy()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if(_isEnabled)
                    Emission();
            }
        }

        IEnumerator UpdateUI()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                if (_isEnabled)
                {
                    var power = Mathf.RoundToInt(_powerSource.GetPower());
                    var powerMax = Mathf.RoundToInt(_powerSource.GetMaxPower());
                    lblStatus.text = $"Power Status : <color={(_isEnabled ? "green": "red")}>{_isEnabled}</color>";
                    lblBattery.text = $"Power {power}/{powerMax} Kw";
                    lblEmission.text = $"{Math.Round(CurrentEmitRate, 2)} w/sec";
                }
                else
                {
                    lblStatus.text = $"Power Status : N/A";
                    lblBattery.text = $"Power 0/0";
                    lblEmission.text = $"0.0 w/sec";
                }
            }
        }

        private void Emission()
        {
            try
            {
                if (Constructable.constructed)
                {
                    //if (!AudioSource.isPlaying)
                    //    this.AudioSource.Play();

                    _powerRelay.ModifyPower(CurrentEmitRate, out float num);

                    //if (_config.TakesDamage && DeepEngineHealth.health - num > 0f)
                    //    DeepEngineHealth.TakeDamage(num / 15f);

                    var power = Mathf.RoundToInt(_powerSource.GetPower());
                    var powerMax = Mathf.RoundToInt(_powerSource.GetMaxPower());
                    lblBattery.text = $"Power {power}/{powerMax}";
                }
            }
            catch (Exception ex)
            {

                Import.LogEvent($"Error Emitting {ex.Message}");
            }
        }

        private void Update()
        {
        }

        void SetupAudio()
        {
            if (!Import.Config.MakesNoise) 
                return;
            AudioClip = Import.Bundle.LoadAsset<AudioClip>("Engine_FX");
            AudioSource = this.gameObject.AddComponent<AudioSource>();
            AudioSource.clip = AudioClip;
            AudioSource.loop = true;
            AudioSource.maxDistance = 15f;
            AudioSource.spatialBlend = 1f;

            if (!AudioSource.isPlaying) 
                AudioSource.Play();
        }

        private void SetEmittedRate()
        {
            var y = this.gameObject.transform.position.y;
            var baseEmission = 1.0f;
            var multiplaier = 4.0f * (float)Import.Config.PowerMultiplier;
            CurrentEmitRate = y >= 0 ? 0.0f : baseEmission + ((y *-1) / 1000.0f) * multiplaier;
        }

        public void OnHandHover(GUIHand hand)
        {
            if (Constructable.constructed)
            {
                if (this.gameObject.transform.position.y <= 0f)
                {
                    var emittedRate = CurrentEmitRate;
                    var power = Mathf.RoundToInt(_powerSource.GetPower());
                    var powerMax = Mathf.RoundToInt(_powerSource.GetMaxPower());

                    HandReticle.main.SetInteractText($"Deep Engine: Current {power}/{powerMax}", $"Producing {Math.Round(CurrentEmitRate, 2)} w/sec", false, false, HandReticle.Hand.None);
                    HandReticle.main.SetIcon(HandReticle.IconType.Info, 1.25f);
                }
                else
                {
                    HandReticle.main.SetInteractText("Deep Engine: ERROR", "Notice: The engine need to be submerged, please relocate", false, false, HandReticle.Hand.None);
                    HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
                }
            }
        }

        public void OnHandClick(GUIHand hand)
        {
        }
    }

}