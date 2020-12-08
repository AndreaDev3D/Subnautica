using AD3D_LightSolutionMod.BO.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AD3D_LightSolutionMod.BO.Utils
{
    public static class Helper
    {
        private static AssetBundle _bundle;
        public static AssetBundle Bundle
        {
            get
            {
                if (_bundle == null)
                {
                    var assetsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");
                    _bundle = AssetBundle.LoadFromFile(Path.Combine(assetsFolder, LightSwitch._AssetName));
                }
                return _bundle;
            }
            set
            {
                _bundle = value;
            }
        }
    }
}
