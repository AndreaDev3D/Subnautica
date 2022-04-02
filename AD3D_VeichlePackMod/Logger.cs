using SMLHelper.V2.Utility;
using UnityEngine;

namespace AD3D_VeichlePackMod
{
    public static class Logger
    {
        public static void Log(string message) => Debug.Log((object)("[VeichlePackMod] " + message));

        public static void Output(string msg) => new BasicText(500, 0).ShowMessage(msg, 5f);
    }
}
