using UnityEngine;
using UnityEditor;

namespace CovaTech.EditorTool
{
    /// <summary>
    /// Utility for Handles class
    /// </summary>
    public static class HandlesUtil
    {
        /// <summary>
        /// Method for Draw WireSphere like Gizmos.DrawWireSphere
        /// </summary>
        /// <param name="_centerPosition">center position of sphere</param>
        /// <param name="_radius">Radius[m]</param>
        public static void DrawWireSphere(Vector3 _centerPosition, float _radius)
        {
            Handles.DrawWireDisc(_centerPosition, Vector3.up, _radius);
            Handles.DrawWireDisc(_centerPosition, Vector3.right, _radius);
            Handles.DrawWireDisc(_centerPosition, Vector3.forward, _radius);
        }

        /// <summary>
        /// Draw WireCone
        /// </summary>
        /// <param name="_origin"> position of conic vertex</param>
        /// <param name="_dir">direction from conic vertex position to center position of base plane</param>
        /// <param name="normalVec">radius direction vector</param>
        /// <param name="_distance"></param>
        /// <param name="_angle">cone angle[deg]</param>
        public static void DrawWireCone(Vector3 _origin, Vector3 _dir, Vector3 normalVec, float _distance, float _angle)
        {
            Vector3 normalizedVDir = Vector3.Normalize(_dir);
            float radius = Mathf.Tan(Mathf.Clamp(_angle, 0f, 360f) * Mathf.Deg2Rad) * _distance;

            // Draw Base circle plane
            Handles.DrawWireDisc(_origin + _distance * normalizedVDir, normalizedVDir, radius);

            // Draw bus
            Vector3 radiusVec = Vector3.Cross(normalizedVDir, normalVec);
            Handles.DrawLine(_origin, _origin + _distance * normalizedVDir + radius * radiusVec);
            Handles.DrawLine(_origin, _origin + _distance * normalizedVDir - radius * radiusVec);
        }
    }
}