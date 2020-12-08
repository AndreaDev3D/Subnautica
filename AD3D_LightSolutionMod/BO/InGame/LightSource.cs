using AD3D_LightSolutionMod.BO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_LightSolutionMod.BO.InGame
{
    public class LightSource : MonoBehaviour, IHandTarget
    {
        public Light Light;
        public int SyncCode = 0;
        private string SyncCodeString;
        // db
        public string _Id;
        private SwitchItem dbItem => QPatch.Database.SwitchItemList.Where(w => w.Id == _Id).FirstOrDefault();
        public virtual string GetPrefabID()
        {
            return gameObject.GetComponent<PrefabIdentifier>()?.Id ?? gameObject.GetComponentInChildren<PrefabIdentifier>()?.Id;
        }
        void Start()
        {
            // Init Db
            _Id = GetPrefabID();
            if (!QPatch.Database.SwitchItemList.Exists(w=>w.Id == _Id)) 
            {
                var newSwitch = new SwitchItem(_Id);
                QPatch.Database.SwitchItemList.Add(newSwitch);
                QPatch.Database.Save();
            }

            Light = AD3D_Common.GameObjectFinder.FindByName(gameObject, "LightItem").GetComponent<Light>();
        }

        public void OnHandClick(GUIHand hand)
        {
        }

        public void OnHandHover(GUIHand hand)
        {
            try
            {
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    SyncCodeString = string.Empty;
                    SyncCode = 0;
                }

                KeyPadDown(KeyCode.Keypad0);
                KeyPadDown(KeyCode.Keypad1);
                KeyPadDown(KeyCode.Keypad2);
                KeyPadDown(KeyCode.Keypad3);
                KeyPadDown(KeyCode.Keypad4);
                KeyPadDown(KeyCode.Keypad5);
                KeyPadDown(KeyCode.Keypad6);
                KeyPadDown(KeyCode.Keypad7);
                KeyPadDown(KeyCode.Keypad8);
                KeyPadDown(KeyCode.Keypad9);

                SyncCode = Convert.ToInt32(SyncCodeString);
                // Save on DB
                dbItem.SetSyncCode(SyncCode);
                QPatch.Database.Save();
            }
            catch (Exception)
            {
                SyncCodeString = string.Empty;
                SyncCode = 0;
            }
            finally
            {
                HandReticle.main.SetInteractText($"Sync Code : {SyncCode}", $"", false, false, HandReticle.Hand.None);
                HandReticle.main.SetIcon(HandReticle.IconType.Info, 1.25f);
            }

        }

        public void KeyPadDown(KeyCode keycode)
        {
            var input = "";
            switch (keycode)
            {                
                case KeyCode.Keypad0:
                    input = "0";
                    break;
                case KeyCode.Keypad1:
                    input = "1";
                    break;
                case KeyCode.Keypad2:
                    input = "2";
                    break;
                case KeyCode.Keypad3:
                    input = "3";
                    break;
                case KeyCode.Keypad4:
                    input = "4";
                    break;
                case KeyCode.Keypad5:
                    input = "5";
                    break;
                case KeyCode.Keypad6:
                    input = "6";
                    break;
                case KeyCode.Keypad7:
                    input = "7";
                    break;
                case KeyCode.Keypad8:
                    input = "8";
                    break;
                case KeyCode.Keypad9:
                    input = "9";
                    break;
                case KeyCode.Alpha0:
                    input = "0";
                    break;
                case KeyCode.Alpha1:
                    input = "1";
                    break;
                case KeyCode.Alpha2:
                    input = "2";
                    break;
                case KeyCode.Alpha3:
                    input = "3";
                    break;
                case KeyCode.Alpha4:
                    input = "4";
                    break;
                case KeyCode.Alpha5:
                    input = "5";
                    break;
                case KeyCode.Alpha6:
                    input = "6";
                    break;
                case KeyCode.Alpha7:
                    input = "7";
                    break;
                case KeyCode.Alpha8:
                    input = "8";
                    break;
                case KeyCode.Alpha9:
                    input = "9";
                    break;
            }

            SyncCodeString = $"{SyncCodeString}{input}";
        }
    }
}