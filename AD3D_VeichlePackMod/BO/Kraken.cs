using AD3D_VeichlePackMod.Utils;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.U2D;
using VehicleFramework;
using VehicleFramework.Engines;
using VehicleFramework.VehicleParts;

namespace AD3D_VeichlePackMod
{
    public class Kraken : ModVehicle
    {
        public const string _Description = "A submarine built for exploration. It is nimble for its size, it fits into small corridors, and its floodlights are extremely powerful.";


        public static GameObject model;
        public static GameObject controlPanel;
        public static Atlas.Sprite pingSprite;

        public static void GetAssets()
        {
            var myLoadedAssetBundle = Utils.Helper.Bundle;
            if (myLoadedAssetBundle == null)
            {
                AD3D_Common.Helper.Log("Failed to load AssetBundle!");
                return;
            }

            var arr = myLoadedAssetBundle.LoadAllAssets();
            AD3D_Common.Helper.Log($"Loading bundle ... {arr}");
            foreach (var obj in arr)
            {
                if (obj.ToString().Contains("AtlasPingSprite"))
                {
                    AD3D_Common.Helper.Log("Loading ... KrakenPingSprite");
                    SpriteAtlas thisAtlas = (SpriteAtlas)obj;
                    Sprite ping = thisAtlas.GetSprite("KrakenPingSprite");
                    pingSprite = new Atlas.Sprite(ping);
                }
                else if (obj.ToString().Contains("Kraken"))
                {
                    AD3D_Common.Helper.Log("Loading ... Kraken");
                    model = (GameObject)obj;
                }
                else if (obj.ToString().Contains("Control-Panel"))
                {
                    AD3D_Common.Helper.Log("Loading ... Control-Panel");
                    controlPanel = (GameObject)obj;
                }
                else
                {
                    AD3D_Common.Helper.Log(obj.ToString());
                }
            }
        }

        public static Dictionary<TechType, int> GetRecipe() => new Dictionary<TechType, int>()
    {
      {
        TechType.TitaniumIngot,
        1
      },
      {
        TechType.PlasteelIngot,
        1
      },
      {
        TechType.Lubricant,
        1
      },
      {
        TechType.AdvancedWiringKit,
        1
      },
      {
        TechType.Lead,
        2
      },
      {
        TechType.EnameledGlass,
        2
      }
    };

        public static void Register()
        {
            GetAssets();
            ModVehicle kraken = model.EnsureComponent<Kraken>() as ModVehicle;
            VehicleManager.RegisterVehicle(ref kraken, new OdysseyEngine(), GetRecipe(), (PingType)122, pingSprite, 8, 0, 600, 667);
        }

        //public override string vehicleDefaultName
        //{
        //    get
        //    {
        //        Language main = Language.main;
        //        return !((Object)main != (Object)null) ? "ODYSSEY" : main.Get("OdysseyDefaultName");
        //    }
        //}

        public override string GetDescription() => _Description;

        public override string GetEncyEntry() => "The Odyssey is a submarine purpose built for exploration. " + "Its maneuverability and illumination capabilities are what earned it the name. \n" + "\nIt features:\n" + "- Modest storage capacity, which can be further expanded with upgrades. \n" + "- Extremely high power flood lights. \n" + "- A signature autopilot which can automatically level out the vessel. \n" + "\nRatings:\n" + "- Top Speed: 12.5m/s \n" + "- Acceleration: 5m/s/s \n" + "- Distance per Power Cell: 7km \n" + "- Crush Depth: 600 \n" + "- Upgrade Slots: 8 \n" + "- Dimensions: 3.7m x 5m x 10.6m \n" + "- Persons: 1-2\n" + "\n\"Don't like it? That's odd; I see.\" ";

        public override GameObject VehicleModel => model;

        public override GameObject StorageRootObject => this.transform.Find(nameof(StorageRootObject)).gameObject;

        public override GameObject ModulesRootObject => this.transform.Find(nameof(ModulesRootObject)).gameObject;

        public override List<VehiclePilotSeat> PilotSeats
        {
            get
            {
                List<VehiclePilotSeat> pilotSeats = new List<VehiclePilotSeat>();
                VehiclePilotSeat vehiclePilotSeat = new VehiclePilotSeat();
                Transform transform = this.transform.Find("Seat");
                vehiclePilotSeat.Seat = transform.gameObject;
                vehiclePilotSeat.SitLocation = transform.Find("SitLocation").gameObject;
                vehiclePilotSeat.LeftHandLocation = transform;
                vehiclePilotSeat.RightHandLocation = transform;
                pilotSeats.Add(vehiclePilotSeat);
                return pilotSeats;
            }
        }

        public override List<VehicleHatchStruct> Hatches
        {
            get
            {
                List<VehicleHatchStruct> hatches = new List<VehicleHatchStruct>();
                VehicleHatchStruct vehicleHatchStruct1 = new VehicleHatchStruct();
                Transform transform1 = this.transform.Find("Hatches/InteriorHatch");
                vehicleHatchStruct1.Hatch = transform1.gameObject;
                vehicleHatchStruct1.EntryLocation = transform1.Find("Entry");
                vehicleHatchStruct1.ExitLocation = transform1.Find("Exit");
                vehicleHatchStruct1.SurfaceExitLocation = transform1.Find("SurfaceExit");
                VehicleHatchStruct vehicleHatchStruct2 = new VehicleHatchStruct();
                Transform transform2 = this.transform.Find("Hatches/ExteriorHatch");
                vehicleHatchStruct2.Hatch = transform2.gameObject;
                vehicleHatchStruct2.EntryLocation = transform2.Find("Entry");
                vehicleHatchStruct2.ExitLocation = transform2.Find("Exit");
                vehicleHatchStruct2.SurfaceExitLocation = transform2.Find("SurfaceExit");
                hatches.Add(vehicleHatchStruct1);
                hatches.Add(vehicleHatchStruct2);
                return hatches;
            }
        }

        public override List<VehicleStorage> InnateStorages
        {
            get
            {
                List<VehicleStorage> innateStorages = new List<VehicleStorage>();
                Transform transform1 = this.transform.Find("InnateStorage/1");
                Transform transform2 = this.transform.Find("InnateStorage/2");
                Transform transform3 = this.transform.Find("InnateStorage/3");
                Transform transform4 = this.transform.Find("InnateStorage/4");
                Transform transform5 = this.transform.Find("InnateStorage/5");
                innateStorages.Add(new VehicleStorage()
                {
                    Container = transform1.gameObject,
                    Height = 6,
                    Width = 5
                });
                innateStorages.Add(new VehicleStorage()
                {
                    Container = transform2.gameObject,
                    Height = 6,
                    Width = 5
                });
                innateStorages.Add(new VehicleStorage()
                {
                    Container = transform3.gameObject,
                    Height = 6,
                    Width = 5
                });
                innateStorages.Add(new VehicleStorage()
                {
                    Container = transform4.gameObject,
                    Height = 6,
                    Width = 5
                });
                innateStorages.Add(new VehicleStorage()
                {
                    Container = transform5.gameObject,
                    Height = 6,
                    Width = 5
                });
                return innateStorages;
            }
        }

        public override List<VehicleStorage> ModularStorages
        {
            get
            {
                List<VehicleStorage> modularStorages = new List<VehicleStorage>();
                for (int index = 1; index <= 8; ++index)
                {
                    VehicleStorage vehicleStorage = new VehicleStorage();
                    Transform transform = this.transform.Find("ModularStorages/" + index.ToString());
                    vehicleStorage.Container = transform.gameObject;
                    vehicleStorage.Height = 4;
                    vehicleStorage.Width = 4;
                    modularStorages.Add(vehicleStorage);
                }
                return modularStorages;
            }
        }

        public override List<VehicleUpgrades> Upgrades => new List<VehicleUpgrades>()
    {
      new VehicleUpgrades()
      {
        Interface = this.transform.Find("Mechanical-Panel/Upgrades-Panel").gameObject
      }
    };

        public override List<VehicleBattery> Batteries => new List<VehicleBattery>()
    {
      new VehicleBattery()
      {
        BatterySlot = this.transform.Find("Mechanical-Panel/BatteryInputs/1").gameObject
      },
      new VehicleBattery()
      {
        BatterySlot = this.transform.Find("Mechanical-Panel/BatteryInputs/2").gameObject
      },
      new VehicleBattery()
      {
        BatterySlot = this.transform.Find("Mechanical-Panel/BatteryInputs/3").gameObject
      },
      new VehicleBattery()
      {
        BatterySlot = this.transform.Find("Mechanical-Panel/BatteryInputs/4").gameObject
      }
    };

        public override List<VehicleBattery> BackupBatteries => new List<VehicleBattery>()
    {
      new VehicleBattery()
      {
        BatterySlot = this.transform.Find("Mechanical-Panel/BatteryInputs/BackupBattery").gameObject
      }
    };

        public override List<VehicleFloodLight> HeadLights => new List<VehicleFloodLight>()
    {
      new VehicleFloodLight()
      {
        Light = this.transform.Find("lights_parent/HeadLights/Left").gameObject,
        Angle = 70f,
        Color = Color.white,
        Intensity = 1.3f,
        Range = 90f
      },
      new VehicleFloodLight()
      {
        Light = this.transform.Find("lights_parent/HeadLights/Right").gameObject,
        Angle = 70f,
        Color = Color.white,
        Intensity = 1.3f,
        Range = 90f
      }
    };

        public override List<VehicleFloodLight> FloodLights
        {
            get
            {
                List<VehicleFloodLight> floodLights = new List<VehicleFloodLight>();
                floodLights.Add(new VehicleFloodLight()
                {
                    Light = this.transform.Find("lights_parent/FloodLights/FrontCenter").gameObject,
                    Angle = 120f,
                    Color = Color.white,
                    Intensity = 1f,
                    Range = 100f
                });
                foreach (Transform transform in this.transform.Find("lights_parent/FloodLights/LateralLights"))
                {
                    VehicleFloodLight vehicleFloodLight = new VehicleFloodLight()
                    {
                        Light = transform.gameObject,
                        Angle = 90f,
                        Color = Color.white,
                        Intensity = 1f,
                        Range = 120f
                    };
                    floodLights.Add(vehicleFloodLight);
                }
                return floodLights;
            }
        }

        public override List<GameObject> NavigationPortLights => (List<GameObject>)null;

        public override List<GameObject> NavigationStarboardLights => (List<GameObject>)null;

        public override List<GameObject> NavigationPositionLights => (List<GameObject>)null;

        public override List<GameObject> NavigationWhiteStrobeLights => (List<GameObject>)null;

        public override List<GameObject> NavigationRedStrobeLights => (List<GameObject>)null;

        public override List<GameObject> WaterClipProxies
        {
            get
            {
                List<GameObject> waterClipProxies = new List<GameObject>();

                //waterClipProxies.Add(this.transform.Find("Models/WaterClipProxies").gameObject);

                foreach (Transform transform in this.transform.Find("WaterClipProxies"))
                    waterClipProxies.Add(transform.gameObject);

                return waterClipProxies;
            }
        }

        public override List<GameObject> CanopyWindows => new List<GameObject>() { this.transform.Find("Models/Canopy").gameObject };

        public override List<GameObject> NameDecals => new List<GameObject>();

        public override List<GameObject> TetherSources
        {
            get
            {
                List<GameObject> tetherSources = new List<GameObject>();
                foreach (Transform transform in this.transform.Find("TetherSources"))
                    tetherSources.Add(transform.gameObject);
                return tetherSources;
            }
        }

        public override GameObject BoundingBox => this.transform.Find("BoundingBox").gameObject;

        public override GameObject ControlPanel
        {
            get
            {
                Kraken.controlPanel.transform.SetParent(this.transform);
                return Kraken.controlPanel;
            }
        }

        public override GameObject CollisionModel => this.transform.Find("CollisionModel").gameObject;
    }
}
