using AD3D_Common.Utils;
using AD3D_LightSolution.BZ;
using AD3D_LightSolution.BZ.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UWE;
using static AD3D_LightSolution.BZ.Base.Enumerators;

namespace AD3D_LightSolution.BZ.Runtime
{
    public class LightSource : MonoBehaviour, IHandTarget, IProtoEventListener
    {
        public static event Action OnSyncLight;

        private PowerRelay powerRelay;

        public string Id => gameObject.GetComponent<PrefabIdentifier>().Id;

        private DataItem DbItem => Plugin.Database.SwitchItemList.FirstOrDefault(w => w.Id == Id);

        private Light _light;
        private Light Light => _light ??= GameObjectFinder.FindByName(gameObject, "LightItem").GetComponent<Light>();

        private int SyncCode;
        private bool IsEnabled;
        private float Intensity;
        private Color LightColor;

        void Awake()
        {
            LightSwitch.OnStatusChanged += OnLightSwitchStatusChanged;
        }

        void Start()
        {
            InitDb();
            ApplyLightSettings(SyncCode, IsEnabled, LightColor, Intensity);

            powerRelay = PowerSource.FindRelay(base.transform);

            powerRelay.powerDownEvent.AddHandler(this, OnPowerDown);
            powerRelay.powerUpEvent.AddHandler(this, OnPowerUp);
        }

        private void OnPowerUp(PowerRelay relay)
        {
            if (relay == powerRelay)
            {
                ApplyLightSettings(SyncCode, true, LightColor, Intensity);
            }
        }

        void OnPowerDown(PowerRelay relay)
        {
            if (relay == powerRelay)
            {
                ApplyLightSettings(SyncCode, false, Color.black, 0.0f);
            }
        }

        private void OnLightSwitchStatusChanged(int syncCode, bool isEnabled, Color color, float intensity)
        {
            ApplyLightSettings(syncCode, isEnabled, color, intensity);
        }

        public void ApplyLightSettings(int syncCode, bool isEnabled, Color color, float intensity)
        {
            if (SyncCode != syncCode) return;

            IsEnabled = isEnabled;
            Intensity = intensity;
            LightColor = color;

            UpdateLightComponent();
            UpdateMaterials();
        }

        private void UpdateLightComponent()
        {
            Light.intensity = IsEnabled ? Intensity : 0.0f;
            Light.color = LightColor;
        }

        private void UpdateMaterials()
        {
            try
            {
                foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
                {
                    foreach (Material material in renderer.materials)
                    {
                        if (material == null) continue;

                        if (IsEnabled)
                        {
                            material.EnableKeyword("MARMO_EMISSION");
                            material.SetColor(ShaderPropertyID._GlowColor, LightColor);
                            material.SetFloat(ShaderPropertyID._GlowStrength, 2.0f);
                            material.SetFloat(ShaderPropertyID._GlowStrengthNight, 3.0f);
                        }
                        else
                        {
                            material.DisableKeyword("MARMO_EMISSION");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Plugin.Logger.LogError($"Error updating materials: {ex}");
            }
        }

        public void OnHandClick(GUIHand hand)
        {
            SyncCode = GetSyncCodeFromClipboard();
            DbItem.SetSyncCode(SyncCode);
            Plugin.Database.Save();
            OnSyncLight?.Invoke();
        }

        public void OnHandHover(GUIHand hand)
        {
            HandReticle.main.SetTextRaw(HandReticle.TextType.Hand, $"Sync Code : {SyncCode}");
            HandReticle.main.SetIcon(HandReticle.IconType.Info, 1.25f);
        }

        private int GetSyncCodeFromClipboard()
        {
            string clipboardText = AD3D_Common.Helper.ClipboardHelper.ClipBoard;
            return int.TryParse(clipboardText, out int syncCode) ? syncCode : 0;
        }

        // Saving
        public void OnProtoSerialize(ProtobufSerializer serializer)
        {
            DbItem.SetSyncCode(SyncCode);
            DbItem.SetEnable(IsEnabled);
            DbItem.SetIntensity(Intensity);
            DbItem.SetColor(LightColor);
            Plugin.Database.Save();
        }

        // Loading
        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {
            InitDb();

            SyncCode = DbItem.SyncCode;
            IsEnabled = DbItem.IsEnable;
            Intensity = DbItem.Intensity;
            LightColor = DbItem.Color;
        }

        public void OnDestroy()
        {
            LightSwitch.OnStatusChanged -= OnLightSwitchStatusChanged;
            Plugin.Logger.LogInfo($"Destroying LightSource with ID: {Id}");
            Plugin.Database.SwitchItemList.Remove(DbItem);
        }

        private void InitDb()
        {
            if (Plugin.Database.SwitchItemList == null)
            {
                Plugin.Database.SwitchItemList = new List<DataItem>();
            }

            if (!Plugin.Database.SwitchItemList.Exists(w => w.Id == Id))
            {
                var newSwitch = new DataItem(Id, SwitchItemType.Source);
                Plugin.Database.SwitchItemList.Add(newSwitch);
            }
        }
    }
}
