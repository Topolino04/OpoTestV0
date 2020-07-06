using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpoTest
{
    public static class Extension
    {

        public static string Truncate(this string str, int length)
        {
            if (str.Length > length) str = str.Substring(length);
            return str;
        }
    }
}
