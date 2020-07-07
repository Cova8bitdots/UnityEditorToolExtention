using System;

namespace CovaTech.Lib
{
    /// <summary>
    /// 128bit XorShift 提供クラス
    /// https://www.jstatsoft.org/article/view/v008i14/xorshift.pdf
    /// </summary>
    public class XorShift : IRandom
    {
        public XorShift() : this((UInt32)DateTime.Now.Ticks)
        {
        }

        public XorShift(UInt32 seed )
        {
            m_seed = seed;
            x = 123456789;
            y = 362436069;
            z = 521288629;
            w = m_seed;
        }

        private UInt32 x, y,z,w;
        private UInt32 m_seed;
        public UInt32 Seed => m_seed;

        public UInt32 Next()
        {
            UInt32 t = x ^ (x << 11);
            x = y;
            y = z;
            z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
            return w;
        }

        public int RandomRange(int min, int max)
        {
            int MAX = Math.Max(min, max);
            int MIN = Math.Min(min, max);
            return (int)((MAX - MIN) * RandomRange(0.0f, 1.0f)) + MIN;
        }

        public float RandomRange(float min, float max)
        {
            float MAX = Math.Max(min, max);
            float MIN = Math.Min(min, max);
            return ((float)Next() / UInt32.MaxValue)*(MAX - MIN) + MIN;
        }
    }
}