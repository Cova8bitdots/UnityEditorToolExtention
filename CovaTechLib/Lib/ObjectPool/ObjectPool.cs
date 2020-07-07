using System.Collections.Generic;
using UnityEngine;

namespace CovaTech.Lib
{
    /// <summary>
    /// IObjectPoolItem 継承アイテムを管理するオブジェクトプール
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ObjectPool<T> : IObjectPool<T> where T: IPoolItem
    {
        //-----------------------------------------------------
        // 定数
        //-----------------------------------------------------
        #region ===== CONSTS =====

        protected const int DEFAULT_POOL_COUNT = 10;

        #endregion //) ===== CONSTS =====

        //-----------------------------------------------------
        // メンバ変数
        //-----------------------------------------------------
        #region ===== MEMBER_VARIABLES =====
        /// <summary>
        /// このObjectPoolで生成したアイテム全て
        /// </summary>
        protected List<T> m_allItems = null;
        /// <summary>
        /// オブジェクトプール置き場
        /// </summary>
        protected Queue<T> m_pool = null;
        /// <summary>
        /// 利用可能なアイテム数
        /// </summary>
        public int CurrentPoolCount => m_pool.Count;
        
        private int m_poolSize = DEFAULT_POOL_COUNT;
        /// <summary>
        /// プールのサイズ
        /// </summary>
        public int MaxPoolSize => m_poolSize;

        protected string m_ItemName = null;
        #endregion //) ===== MEMBER_VARIABLES =====


        //-----------------------------------------------------
        // 初期化
        //-----------------------------------------------------
        #region ===== INITIALIZE =====
        protected void InitializePool(string itemName = null, int _itemCount = DEFAULT_POOL_COUNT)
        {
            m_poolSize = Mathf.Max(0, _itemCount);
            Debug.Assert(m_poolSize > 0);
            m_pool = new Queue<T>(m_poolSize);
            m_allItems = new List<T>(m_poolSize);
            m_ItemName = itemName;
            GeneratePoolItem(m_pool, m_allItems, m_poolSize);
        }

        protected virtual void GeneratePoolItem( Queue<T> _queue, List<T> _itemList, int _itemCount)
        {
            Debug.Assert(_itemCount > 0);
            if ( _itemCount < 1 )
            {
                return;
            }

            for (int i = 0; i < _itemCount; i++)
            {
                var item = CreateItem();
                // 意図しない動作を起こさないようにDisable状態で生成する
                item.SetDisable();
                _queue.Enqueue(item);
                _itemList.Add(item);
                if( !string.IsNullOrEmpty(m_ItemName))
                {
                    item.objectName = m_ItemName+ i.ToString("D3");
                }
            }
        }

        /// <summary>
        /// ObjectPool のリサイズ(Capacity は変更しない )
        /// </summary>
        /// <param name="_newSize"></param>
        public void ResizePoolSize( int _newSize )
        {
            if( _newSize < 1)
            {
                return;
            }
            if( MaxPoolSize > _newSize )
            {
                while (m_pool.Count > _newSize)
                {
                    var item = m_pool.Dequeue();
                    item?.Dispose();
                    m_allItems.Remove(item);
                }
            }
            else
            {
                /*
                 *Capacityに応じての容量増加のため
                 * 無駄に増やさないように極力呼ばない設計を推奨
                 */
                GeneratePoolItem(m_pool, m_allItems, _newSize - MaxPoolSize);
            }
            m_poolSize = m_allItems.Count;
        }

        #endregion //) ===== INITIALIZE =====




        //-----------------------------------------------------
        // 提供メソッド
        //-----------------------------------------------------
        #region ===== IObjectPool =====
        /// <summary>
        /// Item生成
        /// </summary>
        /// <returns></returns>
        public abstract T CreateItem();

        /// <summary>
        /// PoolからItemを拝借
        /// </summary>
        /// <returns>T 型のItem or NULL </returns>
        public T Rent() 
        {
            Debug.Assert(CurrentPoolCount > 0, "Empty Pool !! ExpandQueueSize");
            if(CurrentPoolCount < 1)
            {
                ResizePoolSize(MaxPoolSize + 1);
            }
            T item = m_pool.Dequeue();
            Debug.Assert(item != null);
            if( item != null )
            {
                OnBeforeRent(item);
            }
            return item;
        }

        /// <summary>
        /// 拝借前の設定
        /// </summary>
        /// <param name="_item">Pool からの貸し出し対象</param>
        protected virtual void OnBeforeRent(T _item)
        {

        }


        /// <summary>
        /// Poolへ返却
        /// </summary>
        public void Return(T _item)
        {
            if( _item == null )
            {
                return;
            }
            // 返却前の処理
            OnBeforeReturn(_item);

            m_pool.Enqueue(_item);
        }

        /// <summary>
        /// Item 返却前処理
        /// </summary>
        protected virtual void OnBeforeReturn(T _item)
        {
            // 無効化で動かないようにする
            _item.SetDisable();
        }


        /// <summary>
        /// ハンドラ指定のアイテムを返す
        /// </summary>
        /// <param name="_handler">ハンドラ</param>
        /// <returns></returns>
        public T GetItem(int _handler)
        {
            foreach( var item in m_allItems)
            {
                if( item.GetHandler() == _handler)
                {
                    return item;
                }
            }
            return default(T);
        }
        #endregion //) ===== IObjectPool =====


        //-----------------------------------------------------
        // Dispose
        //-----------------------------------------------------
        #region ===== IDisposable =====

        /// <summary>
        /// 終了処理
        /// </summary>
        public virtual void Dispose()
        {
            while(m_pool.Count > 0)
            {
                m_pool.Dequeue()?.Dispose();
            }
        }

        #endregion //) ===== IDisposable =====
    }
}