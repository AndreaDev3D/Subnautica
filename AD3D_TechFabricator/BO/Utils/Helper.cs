using SMLHelper.V2.Utility;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace AD3D
{
    public class Helper
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

        public static Atlas.Sprite GetSprite(string filename)
        {
            return ImageUtils.LoadSpriteFromFile($"./QMods/{Constant.TechFabricator_ModName}/Assets/{filename}.png");
        }

        public static Texture2D GetTexture(string filename)
        {
            return ImageUtils.LoadTextureFromFile($"./QMods/{Constant.TechFabricator_ModName}/Assets/{filename}.png");
        }

        public static void LogEvent(string text, bool showOnScreen = false)
        {
            QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Info, $"{text} {Constant.Spacer}", showOnScreen: showOnScreen);
        }
    }
}
