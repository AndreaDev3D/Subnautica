using Nautilus.Extensions;
using Nautilus.Json;
using System;
using AD3D_Common.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static RadicalLibrary.Easing;

namespace AD3D_EnergySolution.BO.Runtime
{
    public class Out<T> : IOut<T>
    {
        private T _value;

        public void Set(T value)
        {
            _value = value;
        }

        public T Get()
        {
            return _value;
        }
    }

    public class DeepEngineController : HandTarget, IHandTarget
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

        private Constructable _constructable;
        public Constructable Constructable => _constructable ??= gameObject.GetComponent<Constructable>();

        public bool IsEnabled;

        public int MaxPowerAllowed = 10000;
        public float PowerMultiplier = 2f;
        public bool MakesNoise;
        public AudioClip Engine_SFX;

        private Animation anim;
        public AnimationClip DrillingAnimation;

        override public void Awake()
        {
            // Init UI
            lblStatus = FindByName(this.gameObject, "lblStatus").gameObject.GetComponent<Text>();
            lblBattery = FindByName(this.gameObject, "lblBattery").gameObject.GetComponent<Text>();
            lblEmission = FindByName(this.gameObject, "lblEmission").gameObject.GetComponent<Text>();
            lblDepth = FindByName(this.gameObject, "lblDepth").gameObject.GetComponent<Text>();

            anim = this.gameObject.FindByName("Engine").GetComponent<Animation>();
            anim.AddClip(DrillingAnimation, "Drilling");
        }

        public void Start()
        {
            StartCoroutine(SetupSolarPowerRelay());
        }

        private IEnumerator SetupSolarPowerRelay()
        {
            yield return new WaitForSeconds(5f);
            Out<GameObject> result = new Out<GameObject>();
            yield return CraftData.GetPrefabForTechTypeAsync(TechType.SolarPanel, true, result);

            GameObject prefab = Instantiate(result.Get());

            prefab.SetActive(false);

            PowerRelay solarPowerRelay = prefab.GetComponent<PowerRelay>();

            _powerSource = this.gameObject.AddComponent<PowerSource>().CopyComponent(prefab.GetComponent<PowerSource>());
            _powerSource.maxPower = MaxPowerAllowed;

            _powerFX = this.gameObject.AddComponent<PowerFX>().CopyComponent(prefab.GetComponent<PowerFX>());
            _powerFX.vfxPrefab = solarPowerRelay.powerFX.vfxPrefab;
            _powerFX.attachPoint = this.gameObject.transform;

            _powerRelay = this.gameObject.AddComponent<PowerRelay>().CopyComponent(prefab.GetComponent<PowerRelay>());
            _powerRelay.powerSystemPreviewPrefab = Instantiate(solarPowerRelay.powerSystemPreviewPrefab);
            _powerRelay.powerFX = _powerFX;
            _powerRelay.maxOutboundDistance = 15;
            _powerRelay.internalPowerSource = _powerSource;

            DestroyImmediate(prefab);


            SetupAudio();

            SetEmittedRate();
            lblDepth.text = $"{Mathf.RoundToInt(Mathf.Abs(this.gameObject.transform.position.y)).ToString()} m";

            InvokeRepeating("EmitEnergy", 0, 1);

            StartNStop();
        }

        private void EmitEnergy()
        {
            if (!IsEnabled)
                return;

            try
            {
                if (Constructable.constructed)
                {
                    _powerRelay.ModifyPower(CurrentEmitRate, out float num);

                    var power = Mathf.RoundToInt(_powerSource.GetPower());
                    var powerMax = Mathf.RoundToInt(_powerSource.GetMaxPower());
                    lblBattery.text = $"Power {power}/{powerMax} Kw";
                    
                    UpdateUI();
                }
            }
            catch (Exception ex)
            {
               Plugin.Logger.LogError($"Error Emitting {ex.Message}");
            }
        }

        private void UpdateUI()
        {
            if (IsEnabled)
            {
                var power = Mathf.RoundToInt(_powerSource.GetPower());
                var powerMax = Mathf.RoundToInt(_powerSource.GetMaxPower());
                lblStatus.text = $"Power Status : <color={(IsEnabled ? "green" : "red")}>{IsEnabled}</color>";
                lblBattery.text = $"Power {power}/{powerMax} Kw";
                lblEmission.text = $"{Math.Round(CurrentEmitRate, 2)} w/sec";
            }
            else
            {
                lblStatus.text = $"Power Status : <color=yellow>N/A</color>";
                lblBattery.text = $"Power 0/0  Kw";
                lblEmission.text = $"0.0 w/sec";
            }
        }

        void SetupAudio()
        {
            if (!MakesNoise)
                return;
            AudioClip = Engine_SFX;
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
            var multiplaier = 4.0f * PowerMultiplier;
            CurrentEmitRate = y >= 0 ? 0.0f : baseEmission + ((y * -1) / 1000.0f) * multiplaier;
        }

        public void OnHandHover(GUIHand hand)
        {
            if (Constructable.constructed && _powerSource != null)
            {
                if (transform.position.y <= 0f)
                {
                    var emittedRate = CurrentEmitRate;
                    var power = Mathf.RoundToInt(_powerSource.GetPower());
                    var powerMax = Mathf.RoundToInt(_powerSource.GetMaxPower());
                    // TODO
                    //HandReticle.main.SetText($"Deep Engine: Current {power}/{powerMax}", $"Producing {Math.Round(CurrentEmitRate, 2)} w/sec", false, false);
                    HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, $"Deep Engine: Current {power}/{powerMax} \nProducing {Math.Round(CurrentEmitRate, 2)} w/sec");
                    HandReticle.main.SetIcon(HandReticle.IconType.Info, 1.25f);
                }
                else
                {
                    // TODO
                    //HandReticle.main.SetText(("Deep Engine: ERROR", "Notice: The engine need to be submerged, please relocate", false, false);
                    HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, $"Deep Engine: ERROR \nNotice: The engine need to be submerged, please relocate at lower depth");
                    HandReticle.main.SetIcon(HandReticle.IconType.HandDeny, 1f);
                }
            }
        }

        public void OnHandClick(GUIHand hand)
        {
            StartNStop();
        }

        private void StartNStop()
        {
            IsEnabled = !IsEnabled;
            if (IsEnabled)
            {
                anim.Play("Drilling");
            }
            else
            {
                anim.Stop();
            }
            UpdateUI();
        }

        public GameObject FindByName(GameObject go, string name)
        {
            Transform[] ts = go.GetComponentsInChildren<Transform>();
            foreach (var t in ts)
                if (t.gameObject.name == name) return t.gameObject;

            return null;
        }
    }

}