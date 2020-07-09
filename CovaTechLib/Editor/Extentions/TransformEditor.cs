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

    Quaternion m_localRotation = Quaternion.identity;

    private Editor m_editor = null;
    private Transform m_trans = null;

    private SerializedProperty positionProperty;
    private SerializedProperty rotationProperty;
    private SerializedProperty scaleProperty;

    private void OnEnable()
    {
        var type = typeof(EditorApplication).Assembly.GetType("UnityEditor.TransformInspector");
        m_editor = CreateEditor(targets, type);
        m_trans = target as Transform;

        m_localRotation = m_trans.localRotation;

        this.positionProperty = this.serializedObject.FindProperty("m_LocalPosition");
        this.rotationProperty = this.serializedObject.FindProperty("m_LocalRotation");
        this.scaleProperty    = this.serializedObject.FindProperty("m_LocalScale");
    }
    private void OnDisable()
    {
        DestroyImmediate(m_editor);
        m_editor = null;
    }

    public override void OnInspectorGUI()
    {
        m_localRotation = m_trans.localRotation;

        this.serializedObject.Update();

        // Position
        DrawVec3PropertyField(this.positionProperty, LABEL_LOCAL_POS);            
        DrawDisableVector3Field(m_trans.position, LABEL_WORLD_POS);

        int changeFlag = 0;
        if( targets.Length > 1 )
        {
            EditorGUI.showMixedValue = true;
        }
        EditorGUI.BeginChangeCheck();
        {
            Vector3 angles = m_localRotation.eulerAngles;
            changeFlag = DrawVec3Field(ref angles, LABEL_LOCAL_ROT);
            m_localRotation.eulerAngles = angles;
            DrawDisableVector3Field(m_trans.rotation.eulerAngles, LABEL_WORLD_ROT);
        }
        if( EditorGUI.EndChangeCheck() )
        {
            Undo.RecordObjects(targets, "UpdateRotation");
            foreach( var item in targets)
            {
                EditorUtility.SetDirty(item);
                Transform tr = item as Transform;
                tr.localRotation = m_localRotation;
            }
        }
        EditorGUI.showMixedValue = false;

        // Scale
        DrawVec3PropertyField(this.scaleProperty, LABEL_LOCAL_SCALE);            
        DrawDisableVector3Field(m_trans.lossyScale, LABEL_WORLD_SCALE);

        this.serializedObject.ApplyModifiedProperties();
    }

    private void DrawVec3PropertyField( SerializedProperty _property, string _label)
    {
        using( new EditorGUILayout.HorizontalScope() )
        {
            EditorGUILayout.LabelField(_label, GUILayout.Width(LABEL_WIDTH));
            bool isReset = GUILayout.Button(LABEL_RESET, GUILayout.Width(BUTTON_WIDTH));
            EditorGUILayout.PropertyField(_property, GUIContent.none);

            if (isReset)
            {
                _property.vector3Value = Vector3.zero;
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
