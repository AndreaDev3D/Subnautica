using AD3D_DeepEngineMod.BO.Utils;
using QModManager.Utility;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options;
using SMLHelper.V2.Options.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD3D_DeepEngineMod.BO
{
    [Menu("DeepEngine Settings")]
    public class DeepEngineConfig : ConfigFile
    {
        [Slider("Max Power Allowed", 500, 750, DefaultValue = 500, Step =5,Tooltip ="Max power capacity for each generator")]
        public int MaxPowerAllowed { get; set; }
        [Slider("Power Multiplier", 1, 3, DefaultValue = 1, Tooltip = "Power multiplier for depth algo")]
        public int PowerMultiplier { get; set; }

        [Toggle("Makes Noise"), OnChange(nameof(MyCheckboxToggleEvent))]
        public bool MakesNoise { get; set; } = false;
        [Toggle("Verboso",Tooltip ="Log info in QMod log"), OnChange(nameof(MyCheckboxToggleEvent))]
        public bool LogEvent { get; set; } = true;


        private void MyCheckboxToggleEvent(ToggleChangedEventArgs e)
        {
            // Reload if value changed
            Utils.Helper.LoadConfig();
            // Log
            Logger.Log(Logger.Level.Info, "Config value was changed!");
            Logger.Log(Logger.Level.Info, $"{e.Value}");
        }
    }
}
