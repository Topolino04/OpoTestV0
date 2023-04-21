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

        private static readonly Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
