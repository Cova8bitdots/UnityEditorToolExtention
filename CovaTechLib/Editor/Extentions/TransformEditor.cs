using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(Transform))]
public class TransformEditor : Editor
{
    private const int VEC3_X_FLAG = 0x01;
    private const int VEC3_Y_FLAG = 0x02;
    private const int VEC3_Z_FLAG = 0x04;
    private const int ALL_VEC3_FLAG = 0x07;
    
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
        if( targets == null || targets.Length < 1)
        {
            return;
        }
        Transform trans = target as Transform;
        if (trans == null)
        {
            return;
        }
        m_localPosition = trans.localPosition;
        m_localRotation = trans.localRotation;
        m_localScale = trans.localScale;

        int changeFlag = 0;
        EditorGUI.BeginChangeCheck();
        {
            changeFlag = DrawVec3Field(ref m_localPosition, LABEL_LOCAL_POS);
            DrawDisableVector3Field(trans.position, LABEL_WORLD_POS);
        }
        if( EditorGUI.EndChangeCheck() )
        {
            foreach( var item in targets)
            {
                EditorUtility.SetDirty(item);
                Transform tr = item as Transform;
                Undo.RecordObject(target, "UpdatePosition");
                Vector3 localPos = tr.localPosition;
                localPos.x = (changeFlag & VEC3_X_FLAG) != 0 ? m_localPosition.x : localPos.x;
                localPos.y = (changeFlag & VEC3_Y_FLAG) != 0 ? m_localPosition.y : localPos.y;
                localPos.z = (changeFlag & VEC3_Z_FLAG) != 0 ? m_localPosition.z : localPos.z;
                tr.localPosition = localPos;
            }
        }
            
        EditorGUI.BeginChangeCheck();
        {
            Vector3 angles = m_localRotation.eulerAngles;
            changeFlag = DrawVec3Field(ref angles, LABEL_LOCAL_ROT);
            m_localRotation.eulerAngles = angles;
            DrawDisableVector3Field(trans.rotation.eulerAngles, LABEL_WORLD_ROT);
        }
        if( EditorGUI.EndChangeCheck() )
        {
            foreach( var item in targets)
            {
                EditorUtility.SetDirty(item);
                Transform tr = item as Transform;
                Undo.RecordObject(target, "UpdateRotation");
                tr.localRotation = m_localRotation;
            }
        }
            
        EditorGUI.BeginChangeCheck();
        {
            changeFlag = DrawVec3Field(ref m_localScale, LABEL_LOCAL_SCALE);
            DrawDisableVector3Field(trans.lossyScale, LABEL_WORLD_SCALE);
        }
        if( EditorGUI.EndChangeCheck() )
        {
            foreach( var item in targets)
            {
                EditorUtility.SetDirty(item);
                Transform tr = item as Transform;
                Undo.RecordObject(target, "UpdatePosition");
                Vector3 localScale = tr.localScale;
                localScale.x = (changeFlag & VEC3_X_FLAG) != 0 ? m_localScale.x : localScale.x;
                localScale.y = (changeFlag & VEC3_Y_FLAG) != 0 ? m_localScale.y : localScale.y;
                localScale.z = (changeFlag & VEC3_Z_FLAG) != 0 ? m_localScale.z : localScale.z;
                tr.localScale = localScale;
            }
        }
    }
    /// <summary>
    /// Draw Vector3 Field
    /// </summary>
    /// <param name="_vec"></param>
    /// <param name="_label"></param>
    /// <returns>BitField which shows where modified</returns>
    private int DrawVec3Field( ref Vector3 _vec, string _label)
    {
        int ret = 0;
        using( new EditorGUILayout.HorizontalScope() )
        {
            EditorGUILayout.LabelField(_label, GUILayout.Width(LABEL_WIDTH));
            bool isReset = GUILayout.Button(LABEL_RESET, GUILayout.Width(BUTTON_WIDTH));
            Vector3 newValue = EditorGUILayout.Vector3Field("", _vec);

            ret |= newValue.x != _vec.x ? VEC3_X_FLAG : 0;
            ret |= newValue.y != _vec.y ? VEC3_Y_FLAG : 0;
            ret |= newValue.z != _vec.z ? VEC3_Z_FLAG : 0;
            _vec = newValue;
            if (isReset)
            {
                _vec = Vector3.zero;
                ret |= ALL_VEC3_FLAG;
            }
        }
        return ret;
    }

    /// <summary>
    /// 表示のみのVector3フィールド
    /// </summary>
    /// <param name="_target"></param>
    /// <param name="_label"></param>
    private void DrawDisableVector3Field(Vector3 _target, string _label)
    {
        using( new EditorGUILayout.HorizontalScope() )
        {
            using( new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.LabelField(_label, GUILayout.Width(LABEL_WIDTH + BUTTON_WIDTH));
                EditorGUILayout.Vector3Field("", _target);
            }
        }
    }

}
