using UnityEngine;

namespace CovaTech.Extentions
{
    /// <summary>
    /// GameObjectの拡張
    ///</summary>
    public static class GameObjectExtentions
    {
        /// <summary>
        /// SetActive の改良版
        /// 不必要にSetActiveを呼ばないように拡張
        /// </summary>
        /// <param name="self"></param>
        /// <param name="isActive"></param>
        public static void SetActiveIfRequired(this GameObject self, bool isActive)
        {
            if (self == null || self.activeSelf == isActive)
            {
                return;
            }
            self.SetActive(isActive);
        }

    }
}