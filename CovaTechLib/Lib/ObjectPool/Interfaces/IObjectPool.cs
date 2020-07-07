namespace CovaTech.Lib
{
    /// <summary>
    /// 汎用ObjectPoolのインターフェース
    /// </summary>
    public interface IObjectPool<T> where T : IPoolItem, System.IDisposable
    {
        /// <summary>
        /// Item生成
        /// </summary>
        /// <returns></returns>
        T CreateItem();

        /// <summary>
        /// PoolからItemを拝借
        /// </summary>
        /// <returns> </returns>
        T Rent();

        /// <summary>
        /// Poolへ返却
        /// </summary>
        void Return(T _item);

        /// <summary>
        /// ハンドラ指定のアイテムを返す
        /// </summary>
        /// <param name="_handler">ハンドラ</param>
        /// <returns></returns>
        T GetItem(int _handler);
    }
}