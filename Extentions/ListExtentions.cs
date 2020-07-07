using System.Collections.Generic;

using CovaTech.Lib;

namespace CovaTech.Extentions
{
    /// <summary>
    /// Listの拡張
    ///</summary>
    public static class ListExtention
    {
        public static bool IsNullOrEmpty<T>(this List<T> self)
        {
            return (self == null || self.Count < 1);
        }

        public static void Shuffle<T>(this List<T> self)
        {
            if (self.IsNullOrEmpty())
            {
                return;
            }
            IRandom randProvider = new XorShift();
            //Fisher-Yates Algorithum
            int n = self.Count;
            while (n > 1)
            {
                n--;
                int k = randProvider.RandomRange(0, n + 1);
                T tmp = self[k];
                self[k] = self[n];
                self[n] = tmp;
            }
        }

    }
}