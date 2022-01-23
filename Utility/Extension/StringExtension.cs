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
            var status = (EnumTransactionStatus)stat;
            return status;
        }
    }
}
