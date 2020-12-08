using AD3D_LightSolutionMod.BO.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_LightSolutionMod.BO.InGame
{
    class LightSwitch : MonoBehaviour
    {
        // db
        public string _Id;
        public virtual SwitchItem dbItem => QPatch.Database.SwitchItemList.Where(w => w.Id == _Id).FirstOrDefault();
        public virtual string GetPrefabID() => gameObject.GetComponent<PrefabIdentifier>()?.Id ?? gameObject.GetComponentInChildren<PrefabIdentifier>()?.Id;
        
        // Ingame
        public int SyncCode = 0;
        public bool IsEnable;
        public Color MainColor;
        public List<GameObject> LightSourceList;
        public float CurrentIntensity = 0.0f;
        public float CurrentMaxIntensity = 0.0f;
        public float MinIntensity = 0.0f;
        public float MaxIntensity = 3.0f;
        // UI
        public GameObject MainDisplay;
        public GameObject SettingsDisplay;
        public Text txtIntensity;
        public Button btnSwitch;
        public Button btnOpenSetting;
        public Button btnHome;
        public Text btnSwitchText;
        public Button btnLessPower;
        public Button btnMorePower;
        public Text txtSyncCode;

        public Slider SliderR;
        public Slider SliderG;
        public Slider SliderB;
        public Image ColorPicker;

        void Start()
        {
            // Init Db
            _Id = GetPrefabID();
            if (!QPatch.Database.SwitchItemList.Exists(w => w.Id == _Id))
            {
                var newSwitch = new SwitchItem(_Id);
                QPatch.Database.SwitchItemList.Add(newSwitch);
                QPatch.Database.Save();
            }

            if (SyncCode == 0) SyncCode = GetSyncCode();
            IsEnable = false;
            MainColor = new Color(1, 1, 1, 1);
            InitUI();

            btnSwitch.onClick.AddListener(() => SwitchLight());
            btnLessPower.onClick.AddListener(() => RemoveIntensity());
            btnMorePower.onClick.AddListener(() => AddIntensity());
            btnOpenSetting.onClick.AddListener(() => OpenSetting(true));
            btnHome.onClick.AddListener(() => OpenSetting(false));

            SliderR.onValueChanged.AddListener(delegate { SetMainColor(); });
            SliderG.onValueChanged.AddListener(delegate { SetMainColor(); });
            SliderB.onValueChanged.AddListener(delegate { SetMainColor(); });
            // Hide Settings
            SettingsDisplay.SetActive(false);

            txtSyncCode.text = $"Sync Code: {SyncCode}";
            // Set Text
            CurrentMaxIntensity = CurrentIntensity;
            CurrentIntensity = IsEnable ? MaxIntensity : MinIntensity;
            txtIntensity.text = CurrentIntensity.ToString();
            btnSwitchText.text = IsEnable ? "ON" : "OFF";
            SwitchLightList();

        }

        private void OpenSetting(bool value)
        {
            SettingsDisplay.SetActive(value);
            MainDisplay.SetActive(!value);
        }

        private void SetMainColor()
        {
            MainColor = new Color(SliderR.value, SliderG.value, SliderB.value, 1.0f);
            ColorPicker.color = MainColor;
            SwitchLightList();
            // db entry
            dbItem.SetColor(MainColor.r, MainColor.g, MainColor.b);
            QPatch.Database.Save();
        }

        void InitUI()
        {
            MainDisplay = GameObject.Find("MainDisplay");
            SettingsDisplay = GameObject.Find("SettingsDisplay");

            btnSwitch = GameObject.Find("btnSwitch").GetComponent<Button>();

            btnOpenSetting = GameObject.Find("btnOpenSetting").GetComponent<Button>();
            btnHome = GameObject.Find("btnHome").GetComponent<Button>();

            btnLessPower = GameObject.Find("btnLessPower").GetComponent<Button>();
            btnMorePower = GameObject.Find("btnMorePower").GetComponent<Button>();

            txtIntensity = GameObject.Find("txtIntensity").GetComponent<Text>();
            btnSwitchText = GameObject.Find("btnSwitchText").GetComponent<Text>();
            txtSyncCode = GameObject.Find("txtSyncCode").GetComponent<Text>();

            SliderR = GameObject.Find("SliderR").GetComponent<Slider>();
            SliderG = GameObject.Find("SliderG").GetComponent<Slider>();
            SliderB = GameObject.Find("SliderB").GetComponent<Slider>();

            ColorPicker = GameObject.Find("ColorPicker").GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void SwitchLight()
        {
            IsEnable = !IsEnable;

            if (IsEnable && CurrentIntensity == MinIntensity)
                CurrentIntensity = CurrentMaxIntensity;

            if (!IsEnable)
                CurrentIntensity = MinIntensity;

            txtIntensity.text = CurrentIntensity.ToString();
            btnSwitchText.text = CurrentIntensity == 0 ? "OFF" : "ON";
            SwitchLightList();
        }

        private void SwitchLightList()
        {
            LightSourceList.Clear();
            FindObjectsOfType<LightSource>().ToList().ForEach(e => LightSourceList.Add(e.gameObject));
            foreach (var item in LightSourceList)
            {
                var lightSource = item.GetComponent<LightSource>();
                if (lightSource.SyncCode == SyncCode)
                {
                    lightSource.Light.intensity = CurrentIntensity;
                    lightSource.Light.color = MainColor;
                }
            }
        }

        private void AddIntensity()
        {
            if (CurrentIntensity >= MaxIntensity) return;
            CurrentIntensity += 0.25f;

            CurrentMaxIntensity = CurrentIntensity;
            txtIntensity.text = CurrentIntensity.ToString();
            SwitchLightList();
            // db entry
            dbItem.SetIntensity(CurrentMaxIntensity);
            QPatch.Database.Save();
        }

        private void RemoveIntensity()
        {
            if (CurrentIntensity <= MinIntensity) return;
            CurrentIntensity -= 0.25f;

            CurrentMaxIntensity = CurrentIntensity;
            txtIntensity.text = CurrentIntensity.ToString();
            SwitchLightList();
            // db entry
            dbItem.SetIntensity(CurrentMaxIntensity);
            QPatch.Database.Save();
        }

        private int GetSyncCode()
        {
            var newSyncCode = UnityEngine.Random.Range(1000, 9999);
            var switchList = FindObjectsOfType<LightSwitch>();
            foreach (var item in switchList)
            {
                if (item.SyncCode == newSyncCode)
                {
                    newSyncCode = UnityEngine.Random.Range(1000, 9999);
                }
            }
            // db entry
            dbItem.SetSyncCode(newSyncCode);
            QPatch.Database.Save();
            return newSyncCode;
        }
    }
}
