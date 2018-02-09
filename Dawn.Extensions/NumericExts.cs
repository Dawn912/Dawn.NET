using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Extensions
{
    public static class NumericExts
    {
        public static int ToInt32(this string intStr)
        {
            return ToInt32(intStr, 0);
        }
        public static int ToInt32(this string intStr, int defaultValue)
        {
            if (String.IsNullOrEmpty(intStr))
            {
                return defaultValue;
            }
            int result;
            if (Int32.TryParse(intStr, out result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
