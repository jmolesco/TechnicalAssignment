using System;
using System.Collections.Generic;
using System.Text;

namespace Utility.Extension
{
    public static class StringExtension
    {
        public static bool HasValue(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
