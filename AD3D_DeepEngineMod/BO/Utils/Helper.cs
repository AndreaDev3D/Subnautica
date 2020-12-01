using SMLHelper.V2.Utility;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AD3D_DeepEngineMod.BO.Utils
{
    public class Helper
    {
        public static DeepEngineConfig Config { get; } = new DeepEngineConfig();

        private static AssetBundle _bundle;
        public static AssetBundle Bundle
        {
            get
            {
                if (_bundle == null)
                {
                    var mainDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    var assetsFolder = Path.Combine(mainDirectory, "Assets");
                    _bundle = AssetBundle.LoadFromFile(Path.Combine(assetsFolder, Constant.AssetName));
                }
                return _bundle;
            }
            set
            {
                _bundle = value;
            }
        }

        public static void LoadConfig() => Config.Load();

        public static void SaveConfig() => Config.Save();

        //private static string MainDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static Atlas.Sprite GetImage(string filename)
        {
            return ImageUtils.LoadSpriteFromFile($"./QMods/{Constant.DeepEngineMod_ModName}/Assets/{filename}.png");
        }

        public static Atlas.Sprite GetSprite(string filename)
        {
            return ImageUtils.LoadSpriteFromFile($"./QMods/{Constant.DeepEngineMod_ModName}/Assets/{filename}.png");
        }

        public static Texture2D GetTexture(string filename)
        {
            return ImageUtils.LoadTextureFromFile($"./QMods/{Constant.DeepEngineMod_ModName}/Assets/{filename}.png");
        }

        public static Atlas.Sprite GetSpriteFromBundle(string filename)
        {
            return ImageUtils.LoadSpriteFromTexture(Bundle.LoadAsset<Texture2D>(filename));
        }

        public static void LogEvent(string text, bool showOnScreen = false)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, $"{text} {Constant.Spacer}", showOnScreen:showOnScreen);
        }
    }
}
