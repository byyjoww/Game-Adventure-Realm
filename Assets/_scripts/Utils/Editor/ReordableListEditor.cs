using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public abstract class ReordableListEditor : Editor
{
    protected SerializedProperty m_initializebles;
    protected ReorderableList m_ReorderableList;
    protected abstract string listName { get; }
    protected abstract string headerName { get; }
    protected abstract string elementName { get; }

    protected virtual void OnEnable()
    {
        //Find the list in our ScriptableObject script.
        m_initializebles = serializedObject.FindProperty(listName);

        //Create an instance of our reorderable list.
        m_ReorderableList = new ReorderableList(serializedObject: serializedObject, elements: m_initializebles, draggable: true, displayHeader: true,
            displayAddButton: true, displayRemoveButton: true);

        //Set up the method callback to draw our list header
        m_ReorderableList.drawHeaderCallback = DrawHeaderCallback;

        //Set up the method callback to draw each element in our reorderable list
        m_ReorderableList.drawElementCallback = DrawElementCallback;

        //Set the height of each element.
        m_ReorderableList.elementHeightCallback += ElementHeightCallback;

        //Set up the method callback to define what should happen when we add a new object to our list.
        m_ReorderableList.onAddCallback += OnAddCallback;
    }

    /// <summary>
    /// Draws the header for the reorderable list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, headerName);
    }

    /// <summary>
    /// This methods decides how to draw each element in the list
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="isactive"></param>
    /// <param name="isfocused"></param>
    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        //Draw the list item as a property field, just like Unity does internally.
        EditorGUI.PropertyField(position:
            new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
            element, label: new GUIContent(elementName), includeChildren: true);
    }

    /// <summary>
    /// Calculates the height of a single element in the list.
    /// This is extremely useful when displaying list-items with nested data.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private float ElementHeightCallback(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight =
            EditorGUI.GetPropertyHeight(m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }

    /// <summary>
    /// Defines how a new list element should be created and added to our list.
    /// </summary>
    /// <param name="list"></param>
    private void OnAddCallback(ReorderableList list)
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
    }

    /// <summary>
    /// Draw the Inspector Window
    /// </summary>
    public override void OnInspectorGUI()
    {
        EditorUtility.SetDirty(target);

        serializedObject.Update();

        EditorGUILayout.Space();

        DrawPropertiesExcluding(serializedObject, listName);

        EditorGUILayout.Space();

        m_ReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
}