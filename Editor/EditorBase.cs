using UnityEngine;
using UnityEditor;

namespace cova.EditorTool
{
    /// <summary>
    /// Editorの基本クラス
    /// (自動更新をするように)
    /// </summary>
    public class EditorBase : Editor
    {
        //-----------------------------------------------------
        // GUI 自動再描画
        //-----------------------------------------------------
        #region ===== AUTO_REPAINT =====


        /// <summary>
        /// EditorApplication.update Eventに登録
        /// </summary>
        private void OnEditorUpdate()
        {
            Repaint();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnEnable()
        { 
            EditorApplication.update += OnEditorUpdate;
        }
        protected virtual void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }
        #endregion //) ===== AUTO_REPAINT =====

    }
}