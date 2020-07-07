namespace CovaTech.Lib
{
    /// <summary>
    /// 乱数提供クラス向けインターフェース
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// 次の値を生成
        /// </summary>
        /// <returns></returns>
        UInt32 Next();

        /// <summary>
        /// min 以上max 以下の範囲の乱数を返す
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        float RandomRange(float min, float max);

        /// <summary>
        /// min 以上 max 未満の範囲のint を返す
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        int RandomRange(int min, int max);
    }
}