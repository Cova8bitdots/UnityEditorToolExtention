using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    static readonly string LABEL_LOCAL_POS = "LocalPosition";
    static readonly string LABEL_WORLD_POS = "WorldPosition";
    static readonly string LABEL_LOCAL_ROT = "Rotation";
    static readonly string LABEL_WORLD_ROT = "WorldRotation";
    static readonly string LABEL_LOCAL_SCALE = "Scale";
    static readonly string LABEL_WORLD_SCALE = "WorldScale";
    static readonly string LABEL_RESET = "R";

    const float LABEL_WIDTH = 150f;
    const float BUTTON_WIDTH = 20f;
    Vector3 m_localPosition = Vector3.zero;
    Quaternion m_localRotation = Quaternion.identity;
    Vector3 m_localScale = Vector3.one;

    private void OnEnable()
    {
        Transform trans = target as Transform;
        if(trans == null)
        {
            return;
        }
        m_localPosition = trans.localPosition;
        m_localRotation = trans.localRotation;
        m_localScale = trans.localScale;
    }
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        Transform trans = target as Transform;
        if (trans == null)
        {
            return;
        }
        m_localPosition = trans.localPosition;
        m_localRotation = trans.localRotation;
        m_localScale = trans.localScale;
        EditorGUI.BeginChangeCheck();
        {
            DrawLocalPosition(ref m_localPosition);
            DrawWorldPosition(trans);
            DrawLocalRotation(ref m_localRotation);
            DrawWorldRotation(trans);
            DrawLocalScale(ref m_localScale);
            DrawLossyScale(trans);
        }
        if( EditorGUI.EndChangeCheck() )
        {
            EditorUtility.SetDirty(target);
            Undo.RecordObject(target, "UpdateTransform");
            trans.localPosition = m_localPosition;
            trans.localRotation = m_localRotation;
            trans.localScale = m_localScale;
        }

    }

    private void DrawLocalPosition(ref Vector3 _target)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(LABEL_LOCAL_POS, GUILayout.Width(LABEL_WIDTH));
            bool isReset = GUILayout.Button(LABEL_RESET, GUILayout.Width(BUTTON_WIDTH));

            
            _target = EditorGUILayout.Vector3Field("", _target);
            if (isReset)
            {
                _target = Vector3.zero;
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// World座標系の位置を表示
    /// </summary>
    /// <param name="_target"></param>
    private void DrawWorldPosition(Transform _target)
    {
        EditorGUILayout.BeginHorizontal();
        {
            //EditorGUILayout.BeginToggleGroup(LABEL_WORLD_POS, false);
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.LabelField(LABEL_WORLD_POS, GUILayout.Width(LABEL_WIDTH + BUTTON_WIDTH));
                EditorGUILayout.Vector3Field("", _target.position);
            }
            EditorGUI.EndDisabledGroup();
            //EditorGUILayout.EndToggleGroup();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawLocalRotation(ref Quaternion _target)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(LABEL_LOCAL_ROT, GUILayout.Width(LABEL_WIDTH));
            bool isReset = GUILayout.Button(LABEL_RESET, GUILayout.Width(BUTTON_WIDTH));
            Vector3 newAngle = EditorGUILayout.Vector3Field("", _target.eulerAngles);
            if (isReset)
            {
                newAngle = Vector3.zero;
            }
            _target = Quaternion.Euler(newAngle);
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// ワールド座標系での回転角を表示
    /// </summary>
    /// <param name="_target"></param>
    private void DrawWorldRotation(Transform _target)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUI.BeginDisabledGroup(true);
            {
                EditorGUILayout.LabelField(LABEL_WORLD_ROT, GUILayout.Width(LABEL_WIDTH + BUTTON_WIDTH));
                EditorGUILayout.Vector3Field("", _target.rotation.eulerAngles);
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawLocalScale(ref Vector3 _target)
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField(LABEL_LOCAL_SCALE, GUILayout.Width(LABEL_WIDTH));
            bool isReset = GUILayout.Button(LABEL_RESET, GUILayout.Width(BUTTON_WIDTH));
            _target = EditorGUILayout.Vector3Field("", _target);
            if (isReset)
            {
                _target = Vector3.one;
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// ワールド座標系でのScaleを表示
    /// </summary>
    /// <param name="_target"></param>
    private void DrawLossyScale(Transform _target)
    {
        using( new EditorGUILayout.HorizontalScope())
        {
            using( new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.LabelField(LABEL_WORLD_SCALE, GUILayout.Width(LABEL_WIDTH + BUTTON_WIDTH));
                EditorGUILayout.Vector3Field("", _target.lossyScale);
            }
        }
    }
}
