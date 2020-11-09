using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[CustomEditor(typeof(FlexibleGridLayoutGroup), true)]
[CanEditMultipleObjects]
public class FlexibleLayoutGroupEditor : UnityEditor.Editor
{

    SerializedProperty m_Padding;
    SerializedProperty m_Spacing;
    SerializedProperty m_StartCorner;
    SerializedProperty m_StartAxis;
    SerializedProperty m_ChildAlignment;
    SerializedProperty m_ColumnCount;
    SerializedProperty m_RowCount;

    protected virtual void OnEnable()
    {
        m_Padding = serializedObject.FindProperty("m_Padding");
        m_Spacing = serializedObject.FindProperty("m_Spacing");
        m_StartCorner = serializedObject.FindProperty("m_StartCorner");
        m_StartAxis = serializedObject.FindProperty("m_StartAxis");
        m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
        m_ColumnCount = serializedObject.FindProperty("ColumnCount");
        m_RowCount = serializedObject.FindProperty("RowCount");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Padding, true);
        EditorGUILayout.PropertyField(m_Spacing, true);
        EditorGUILayout.PropertyField(m_StartCorner, true);
        EditorGUILayout.PropertyField(m_StartAxis, true);
        EditorGUILayout.PropertyField(m_ChildAlignment, true);
        EditorGUILayout.PropertyField(m_ColumnCount, true);
        EditorGUILayout.PropertyField(m_RowCount, true);
        serializedObject.ApplyModifiedProperties();
    }
}