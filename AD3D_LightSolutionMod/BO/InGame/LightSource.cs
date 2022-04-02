using AD3D_LightSolutionMod.BO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AD3D_LightSolutionMod.BO.InGame
{
    public class LightSource : MonoBehaviour, IHandTarget, IProtoEventListener
    {
        // Events
        public delegate void SyncLight();
        public static event SyncLight OnSyncLight;
        // db
        public string _Id => gameObject.GetComponent<PrefabIdentifier>()?.Id ?? gameObject.GetComponentInChildren<PrefabIdentifier>().Id;
        private DataItem dbItem => QPatch.Database.SwitchItemList.FirstOrDefault(w => w.Id == _Id);

        // Ingame
        private Light Light => AD3D_Common.GameObjectFinder.FindByName(gameObject, "LightItem").GetComponent<Light>();
        private int SyncCode;
        private bool IsEnable;
        private float Intensity;
        private Color Color;
        private string SyncCodeString;

        void Awake()
        {
            LightSwitch.OnStatusChanged += LightSwitch_OnStatusChanged;
        }

        void Start()
        {
            InitDb();
            SetLight(SyncCode, IsEnable, Color, Intensity);
        }

        private void LightSwitch_OnStatusChanged(int syncCode, bool isEnable, Color color, float intensity) => SetLight(syncCode, isEnable, color, intensity);

        public void SetLight(int syncCode, bool isEnable, Color color, float intensity)
        {
            if (SyncCode != syncCode) return;
            try
            {
                // Property
                IsEnable = isEnable;
                Intensity = intensity;
                Color = color;
                // Light
                Light.intensity = isEnable ? intensity : 0.0f;
                Light.color = color;

                List<Renderer> Renderers = gameObject.GetComponentsInChildren<Renderer>().ToList();

                foreach (Renderer renderer in Renderers)
                {
                    foreach (Material material in renderer.materials)
                    {
                        if (material == null) continue;
                        if (!isEnable)
                            material.DisableKeyword("MARMO_EMISSION");
                        else
                            material.EnableKeyword("MARMO_EMISSION");

                        material.SetColor(ShaderPropertyID._GlowColor, color);
                        material.SetFloat(ShaderPropertyID._GlowStrength, 2.0f);
                        material.SetFloat(ShaderPropertyID._GlowStrengthNight, 3.0f);
                    }
                }
            }
            catch (Exception ex)
            {
                AD3D_Common.Helper.Log($"SetLight Error: {ex}", true);
            }
        }

        public void OnHandClick(GUIHand hand)
        {
            SyncCodeString = AD3D_Common.Helper.ClipboardHelper.ClipBoard;
            SyncCode = Convert.ToInt32(SyncCodeString);
            dbItem.SetSyncCode(SyncCode);
            QPatch.Database.Save();
            OnSyncLight?.Invoke();
        }

        public void OnHandHover(GUIHand hand)
        {
            //try
            //{
            //    if (Input.GetKeyDown(KeyCode.Backspace))
            //    {
            //        SyncCodeString = string.Empty;
            //        SyncCode = 0;
            //    }

            //    KeyPadDown(KeyCode.Alpha0);
            //    KeyPadDown(KeyCode.Alpha1);
            //    KeyPadDown(KeyCode.Alpha2);
            //    KeyPadDown(KeyCode.Alpha3);
            //    KeyPadDown(KeyCode.Alpha4);
            //    KeyPadDown(KeyCode.Alpha5);
            //    KeyPadDown(KeyCode.Alpha6);
            //    KeyPadDown(KeyCode.Alpha7);
            //    KeyPadDown(KeyCode.Alpha8);
            //    KeyPadDown(KeyCode.Alpha9);

            //    KeyPadDown(KeyCode.Keypad0);
            //    KeyPadDown(KeyCode.Keypad1);
            //    KeyPadDown(KeyCode.Keypad2);
            //    KeyPadDown(KeyCode.Keypad3);
            //    KeyPadDown(KeyCode.Keypad4);
            //    KeyPadDown(KeyCode.Keypad5);
            //    KeyPadDown(KeyCode.Keypad6);
            //    KeyPadDown(KeyCode.Keypad7);
            //    KeyPadDown(KeyCode.Keypad8);
            //    KeyPadDown(KeyCode.Keypad9);

            //    SyncCode = Convert.ToInt32(SyncCodeString);
            //}
            //catch (Exception)
            //{
            //    SyncCodeString = string.Empty;
            //    SyncCode = 0;
            //}
            //finally
            //{
            HandReticle.main.SetInteractText($"Sync Code : {SyncCode}", $"", false, false, HandReticle.Hand.None);
            HandReticle.main.SetIcon(HandReticle.IconType.Info, 1.25f);
            //}

        }

        public void KeyPadDown(KeyCode keycode)
        {
            //if (Input.GetKeyDown(keycode))
            //{
            //    var input = "";
            //    switch (keycode)
            //    {
            //        case KeyCode.Keypad0:
            //        case KeyCode.Alpha0:
            //            input = "0";
            //            break;
            //        case KeyCode.Keypad1:
            //        case KeyCode.Alpha1:
            //            input = "1";
            //            break;
            //        case KeyCode.Keypad2:
            //        case KeyCode.Alpha2:
            //            input = "2";
            //            break;
            //        case KeyCode.Keypad3:
            //        case KeyCode.Alpha3:
            //            input = "3";
            //            break;
            //        case KeyCode.Keypad4:
            //        case KeyCode.Alpha4:
            //            input = "4";
            //            break;
            //        case KeyCode.Keypad5:
            //        case KeyCode.Alpha5:
            //            input = "5";
            //            break;
            //        case KeyCode.Keypad6:
            //        case KeyCode.Alpha6:
            //            input = "6";
            //            break;
            //        case KeyCode.Keypad7:
            //        case KeyCode.Alpha7:
            //            input = "7";
            //            break;
            //        case KeyCode.Keypad8:
            //        case KeyCode.Alpha8:
            //            input = "8";
            //            break;
            //        case KeyCode.Keypad9:
            //        case KeyCode.Alpha9:
            //            input = "9";
            //            break;
            //    }
            //    SyncCodeString = $"{SyncCodeString}{input}";
            //}
        }

        private void InitDb()
        {
            if (!QPatch.Database.SwitchItemList.Any(w => w.Id == _Id))
            {
                var newSwitch = new DataItem(_Id, SwitchItemType.Source);
                QPatch.Database.SwitchItemList.Add(newSwitch);
            }
        }
        // Saving
        public void OnProtoSerialize(ProtobufSerializer serializer)
        {
            dbItem.SetSyncCode(SyncCode);
            dbItem.SetEnable(IsEnable);
            dbItem.SetIntensity(Intensity);
            dbItem.SetColor(Color);
            QPatch.Database.Save();
        }

        // Loading
        public void OnProtoDeserialize(ProtobufSerializer serializer)
        {
            // Init Db
            InitDb();

            //
            SyncCode = dbItem.SyncCode;
            IsEnable = dbItem.IsEnable;
            Intensity = dbItem.Intensity;
            Color = dbItem.Color;

            //AD3D_Common.Helper.Log($"Prop : {SyncCode} | {IsEnable} @ {Intensity}", true);
        }
    }
}