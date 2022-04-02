using AD3D_LightSolutionMod.BO.Base;
using AD3D_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace AD3D_LightSolutionMod.BO.InGame
{
    public class LightSwitch : MonoBehaviour, IProtoEventListener
    {
        // Events
        public delegate void StatusChanged(int SyncCode, bool IsEnable, Color MainColor, float Intensity);
        public static event StatusChanged OnStatusChanged;
        // db
        public string _Id => gameObject.GetComponent<PrefabIdentifier>()?.Id ?? gameObject.GetComponentInChildren<PrefabIdentifier>().Id;
        public DataItem dbItem => QPatch.Database.SwitchItemList.FirstOrDefault(w => w.Id == _Id);

        // Ingame
        public int SyncCode = 0;
        public bool IsEnable;
        public Color Color;
        public float Intensity = 0.5f;
        //
        public float MinIntensity = 0.5f;
        public float MaxIntensity = 3.0f;
        // UI
        public GameObject MainDisplay;
        public GameObject SettingsDisplay;
        public Text txtIntensity;

        public Button btnSwitch;
        public Image btnSwitchImage;
        public Sprite btnOn;
        public Sprite btnOff;

        public Button btnOpenSetting;
        public Button btnHome;
        public Button btnBasePower;
        public Button btnLessPower;
        public Button btnMorePower;
        public Text txtSyncCode;
        public Button btnSyncCode;

        public Slider SliderR;
        public Slider SliderG;
        public Slider SliderB;
        public Image ColorPicker;

        private void Awake()
        {
            //QPatch.Database.Load();
        }

        private void InitUI()
        {
            MainDisplay = GameObjectFinder.FindByName(this.gameObject, "MainDisplay");
            SettingsDisplay = GameObjectFinder.FindByName(this.gameObject, "SettingsDisplay");

            btnSwitch = GameObjectFinder.FindByName(this.gameObject, "btnSwitch").GetComponent<Button>();
            btnSwitchImage = GameObjectFinder.FindByName(this.gameObject, "btnSwitch").GetComponent<Image>();

            var btnOnTex = AD3D_Common.Helper.GetTextureFromBundle(AD3D_LightSolutionMod.BO.Utils.Helper.Bundle, "btnOn");
            var btnOffTex = AD3D_Common.Helper.GetTextureFromBundle(AD3D_LightSolutionMod.BO.Utils.Helper.Bundle, "btnOff");
            btnOn = Sprite.Create(btnOnTex, new Rect(0.0f, 95.0f, 529.0f, 322.0f), new Vector2(0.5f, 0.5f));
            btnOff = Sprite.Create(btnOffTex, new Rect(0.0f, 95.0f, 529.0f, 322.0f), new Vector2(0.5f, 0.5f));


            btnOpenSetting = GameObjectFinder.FindByName(this.gameObject, "btnOpenSetting").GetComponent<Button>();
            btnHome = GameObjectFinder.FindByName(this.gameObject, "btnHome").GetComponent<Button>();
            btnBasePower = GameObjectFinder.FindByName(this.gameObject, "btnBasePower").GetComponent<Button>();

            btnLessPower = GameObjectFinder.FindByName(this.gameObject, "btnLessPower").GetComponent<Button>();
            btnMorePower = GameObjectFinder.FindByName(this.gameObject, "btnMorePower").GetComponent<Button>();

            txtIntensity = GameObjectFinder.FindByName(this.gameObject, "txtIntensity").GetComponent<Text>();
            txtSyncCode = GameObjectFinder.FindByName(this.gameObject, "txtSyncCode").GetComponent<Text>();
            btnSyncCode = GameObjectFinder.FindByName(this.gameObject, "btnSyncCode").GetComponent<Button>();

            SliderR = GameObjectFinder.FindByName(this.gameObject, "SliderR").GetComponent<Slider>();
            SliderG = GameObjectFinder.FindByName(this.gameObject, "SliderG").GetComponent<Slider>();
            SliderB = GameObjectFinder.FindByName(this.gameObject, "SliderB").GetComponent<Slider>();

            ColorPicker = GameObjectFinder.FindByName(this.gameObject, "ColorPicker").GetComponent<Image>();
        }

        void Start()
        {
            InitDb();
            InitUI();


            btnSwitch.onClick.AddListener(() => SwitchLight());
            btnSyncCode.onClick.AddListener(() => CopyToSyncCode());
            btnLessPower.onClick.AddListener(() => SetIntensity(-0.25f));
            btnMorePower.onClick.AddListener(() => SetIntensity(0.25f));
            btnOpenSetting.onClick.AddListener(() => OpenSetting(true));
            btnHome.onClick.AddListener(() => OpenSetting(false));
            btnBasePower.onClick.AddListener(() => SwitchBaseLight());

            InitOBJ();

            SliderR.onValueChanged.AddListener((float v) => SetMainColor(v));
            SliderG.onValueChanged.AddListener((float v) => SetMainColor(v));
            SliderB.onValueChanged.AddListener((float v) => SetMainColor(v));


            LightSource.OnSyncLight += LightSource_OnSyncLight;
        }

        private void LightSource_OnSyncLight()
        {
            OnStatusChanged?.Invoke(SyncCode, IsEnable, Color, Intensity);
        }

        void InitOBJ()
        {

            SliderR.value = Color.r;
            SliderG.value = Color.g;
            SliderB.value = Color.b;
            ColorPicker.color = Color;

            // Hide Settings
            OpenSetting(false);

            // Set Text
            txtSyncCode.text = $"Sync Code: {SyncCode}";
            btnSwitchImage.sprite = IsEnable ? btnOn : btnOff;
            txtIntensity.text = Intensity.ToString();
        }

        private void CopyToSyncCode()
        {
            GUIUtility.systemCopyBuffer = dbItem.SyncCode.ToString();
        }

        private void OpenSetting(bool value)
        {
            SettingsDisplay.SetActive(value);
            MainDisplay.SetActive(!value);
        }

        private void SwitchLight()
        {
            IsEnable = !IsEnable;

            txtIntensity.text = Intensity.ToString();
            btnSwitchImage.sprite = IsEnable ? btnOn : btnOff;
            // Send Event
            OnStatusChanged?.Invoke(SyncCode, IsEnable, Color, Intensity);
        }

        private void SwitchBaseLight()
        {
            if (!this.enabled)
                return;
            SubRoot currentSub = Player.main.GetCurrentSub();
            if (currentSub == null)
                return;
            Constructable component = this.GetComponent<Constructable>();
            if (component && !component.constructed)
                return;
            bool flag = (bool)typeof(SubRoot).GetField("subLightsOn", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object)currentSub);
            currentSub.ForceLightingState(!flag);
        }

        private void SetMainColor(float v)
        {
            Color = new Color(SliderR.value, SliderG.value, SliderB.value, 1.0f);
            ColorPicker.color = Color;
            // Send Event
            OnStatusChanged?.Invoke(SyncCode, IsEnable, Color, Intensity);
        }

        private void SetIntensity(float value)
        {
            if (Intensity <= MinIntensity && Intensity >= MaxIntensity) return;
            Intensity += value;

            txtIntensity.text = Intensity.ToString();
            // Send Event
            OnStatusChanged?.Invoke(SyncCode, IsEnable, Color, Intensity);
        }

        private void InitDb()
        {
            if (!QPatch.Database.SwitchItemList.Any(w => w.Id == _Id))
            {
                var newSwitch = new DataItem(_Id, SwitchItemType.Switch);
                QPatch.Database.SwitchItemList.Add(newSwitch);
            }
            dbItem.SecurityCheck();
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
            Color = new Color(dbItem.R, dbItem.G, dbItem.B, 1.0f);

            //AD3D_Common.Helper.Log($"Prop : {SyncCode} | {IsEnable} @ {Intensity} Color({Color.r},{Color.g},{Color.b})", true);
        }
    }
}
