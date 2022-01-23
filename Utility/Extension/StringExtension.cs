using System;
using System.Collections.Generic;
using System.Text;
using Utility.Enums;

namespace Utility.Extension
{
    public static class StringExtension
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static object GetTransactionStatus(this int stat)
        {
            int actualStat = (stat == 2 || stat == 4) ? 2 : (stat == 3 || stat == 5) ? 3 : 1;
            var status = (EnumTransactionStatus)actualStat;
            return status;
        }
    }
}
