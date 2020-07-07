using System;
namespace CovaTech.Lib
{
    /// <summary>
    /// ObjectPoolItemのインターフェース
    /// </summary>
    public interface IPoolItem : IDisposable
    {
        /// <summary>
        /// オブジェクト名
        /// </summary>
        /// <value></value>
        string objectName{get;set;}

        /// <summary>
        /// ハンドラ(idだったり) を返すメソッド
        /// </summary>
        /// <returns></returns>
        int GetHandler();

        /// <summary>
        /// 使用中判定メソッド
        /// </summary>
        /// <returns>現在使用中かどうか</returns>
        bool IsUsed();

        /// <summary>
        /// Item の有効化
        /// </summary>
        /// <returns>有効化に成功したかどうか</returns>
        bool SetEnable();

        /// <summary>
        /// Item の無効化
        /// </summary>
        /// <param name="_forceDisable">強制無効化フラグ</param>
        /// <returns>無効化に成功したかどうか</returns>
        bool SetDisable(bool _forceDisable = false);
    }
}