using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD3D_DeepEngineMod.BO.Helper
{
    public class Constant
    {
        public const string Spacer = "<---⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫⚫";
        
        public const string DeepEngineMod_Version = "1.2.0";
        public const string DeepEngineMod_ModName = "AD3D_DeepEngineMod";

        public const string AssetName = "deepengineasset";
        public const string ClassID = "DeepEngine";
        public const string FriendlyName = "Deep Engine MK1";
        public const string ShortDescription = "High efficiency electric generator that runs in deep water.";

        public const string PDAKey = ClassID;

        public static string PDADescription(int MaxPower)
        {
            return $"The Deep Engine MK1 is a depth pressure generator, helps to provide electricity for new bases in hostile territory where sun or wind don't reach.{Environment.NewLine}Producing a max power of {MaxPower}W is capabilities is pretty much unlimited, since no maintenance is required is even pretty autonomous.{Environment.NewLine}You can place it outside the base or on a 15m range from the base.";
        }
    }
}
