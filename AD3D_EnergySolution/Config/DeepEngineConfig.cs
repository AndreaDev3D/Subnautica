using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;

namespace AD3D_EnergySolution.Config
{
    [Menu("DeepEngine Settings")]
    public class DeepEngineConfig : ConfigFile
    {
        [Slider("Max Power Allowed", 500, 750, DefaultValue = 500, Step = 5, Tooltip = "Max power capacity for each generator")]
        public int MaxPowerAllowed { get; set; }
        [Slider("Power Multiplier", 1, 3, DefaultValue = 1, Tooltip = "Power multiplier for depth algo")]
        public int PowerMultiplier { get; set; }

        [Toggle("Makes Noise"), OnChange(nameof(ConfigChanged))]
        public bool MakesNoise { get; set; } = false;
        [Toggle("Verboso", Tooltip = "Log info in QMod log"), OnChange(nameof(ConfigChanged))]
        public bool LogEvent { get; set; } = true;


        private void ConfigChanged(ToggleChangedEventArgs e)
        {
            // Reload if value changed
            Plugin.DeepEngineConfig.Load();
            // Log
            //Logger.Log(Logger.Level.Info, "Config value was changed!");
            //Logger.Log(Logger.Level.Info, $"{e.Value}");
        }
    }
}
