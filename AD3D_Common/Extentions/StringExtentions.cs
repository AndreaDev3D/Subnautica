using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AD3D_Common.Extentions
{
    public static class StringExtentions
    {
        public static string RemoveSuffix(this string source)
        {
            return source.Replace(AD3D_Common.Constant.Suffix, "");
        }
    }
}
