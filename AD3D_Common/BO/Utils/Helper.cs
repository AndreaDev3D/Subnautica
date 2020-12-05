using SMLHelper.V2.Utility;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AD3D_Common
{
    public static class Helper
    {
        /// <summary>
        /// request a file image from the assets folder (only .png format)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        //public static Atlas.Sprite GetImageFromName(string name)
        //{
        //    return ImageUtils.LoadSpriteFromFile($"./QMods/{Constant.TechFabricator_ModName}/Assets/{name}.png");
        //}

        public static GameObject GetGameObjectFromBundle(AssetBundle Bundle, string filename)
        {
            return Bundle.LoadAsset<GameObject>(filename);
        }

        public static Atlas.Sprite GetSpriteFromBundle(AssetBundle Bundle, string filename)
        {
            return ImageUtils.LoadSpriteFromTexture(Bundle.LoadAsset<Texture2D>(filename));
        }

        public static Atlas.Sprite GetSprite(string modName, string filename, string format = "png")
        {
            return ImageUtils.LoadSpriteFromFile($"./QMods/{modName}/Assets/{filename}.{format}");
        }

        public static Texture2D GetTexture(string modName, string filename)
        {
            return ImageUtils.LoadTextureFromFile($"./QMods/{modName}/Assets/{filename}.png");
        }

        public static void Log(string text, bool showOnScreen = false, QModManager.Utility.Logger.Level loggerLevel = QModManager.Utility.Logger.Level.Info)
        {
            QModManager.Utility.Logger.Log(loggerLevel, $"{text} {Constant.Spacer}", showOnScreen: showOnScreen);
        }
    }
}
