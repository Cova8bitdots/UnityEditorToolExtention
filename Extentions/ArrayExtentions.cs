using CovaTech.Lib;

namespace CovaTech.Extentions
{
    /// <summary>
    /// Array オブジェクトの拡張
    /// </summary>
    public static class ArrayExtention
    {
        public static bool IsNullOrEmpty<T>(this T[] self)
        {
            return (self == null || self.Length < 1);
        }

        public static void Shuffle<T>( this T[] self)
        {
            if( self.IsNullOrEmpty() )
            {
                return;
            }

            IRandom randProvider = new XorShift();
            //Fisher-Yates Algorithum
            int n = self.Length;
            while (n > 1)
            {
                n--;
                int k = randProvider.RandomRange(0, n+1);
                T tmp = self[k];
                self[k] = self[n];
                self[n] = tmp;
            }
        }

    }
}