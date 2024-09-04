using Nautilus.Json;
using Nautilus.Options;
using Nautilus.Options.Attributes;
using System;

namespace AD3D_EnergySolution.BZ.Config
{
    [Menu(PluginInfo.PLUGIN_NAME +" Settings")]
    public class DeepEngineConfig : ConfigFile
    {
        public Action OnConfigChanged;

        [Slider("Max Power Allowed", 500, 750, DefaultValue = 500, Step = 5, Tooltip = "Max power capacity for each generator")]
        public int MaxPowerAllowed { get; set; } = 500;

        [Slider("Power Multiplier", 1, 3, DefaultValue = 1, Tooltip = "Power multiplier for depth algorithm")]
        public int PowerMultiplier { get; set; } = 1;

        [Toggle("Makes Noise"), OnChange(nameof(ConfigChanged))]
        public bool MakesNoise { get; set; } = false;

        [Toggle("Verboso", Tooltip = "Log info in log"), OnChange(nameof(ConfigChanged))]
        public bool LogEvent { get; set; } = true;

        private void ConfigChanged(ToggleChangedEventArgs e)
        {
            Plugin.DeepEngineConfig.Load();
            OnConfigChanged?.Invoke();
        }
    }
}
