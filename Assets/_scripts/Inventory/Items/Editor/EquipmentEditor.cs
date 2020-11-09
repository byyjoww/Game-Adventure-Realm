using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Equipment))]
public class EquipmentEditor : ReordableListEditor
{
    protected override string listName => "equipmentStats";
    protected override string headerName => "Stats";
    protected override string elementName => "Stat";

    private SerializedProperty m_initializebles2;
    private ReorderableList m_ReorderableList2;

    protected virtual string secondListName => "equipmentParameters";
    protected virtual string secondListHeaderName => "Parameters";
    protected virtual string secondListElementName => "Parameter";

    protected override void OnEnable()
    {
        base.OnEnable();

        //Find the list in our ScriptableObject script.
        m_initializebles2 = serializedObject.FindProperty(secondListName);

        //Create an instance of our reorderable list.
        m_ReorderableList2 = new ReorderableList(serializedObject: serializedObject, elements: m_initializebles2, draggable: true, displayHeader: true,
            displayAddButton: true, displayRemoveButton: true);

        //Set up the method callback to draw our list header
        m_ReorderableList2.drawHeaderCallback = DrawHeaderCallback;

        //Set up the method callback to draw each element in our reorderable list
        m_ReorderableList2.drawElementCallback = DrawElementCallback;

        //Set the height of each element.
        m_ReorderableList2.elementHeightCallback += ElementHeightCallback;

        //Set up the method callback to define what should happen when we add a new object to our list.
        m_ReorderableList2.onAddCallback += OnAddCallback;
    }

    /// <summary>
    /// Draws the header for the reorderable list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, secondListHeaderName);
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
        SerializedProperty element = m_ReorderableList2.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        //Draw the list item as a property field, just like Unity does internally.
        EditorGUI.PropertyField(position:
            new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
            element, label: new GUIContent(secondListElementName), includeChildren: true);
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
            EditorGUI.GetPropertyHeight(m_ReorderableList2.serializedProperty.GetArrayElementAtIndex(index), true);

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
        serializedObject.Update();

        EditorGUILayout.Space();

        DrawPropertiesExcluding(serializedObject, new string[2] { listName, secondListName });

        EditorGUILayout.Space();

        m_ReorderableList.DoLayoutList();
        m_ReorderableList2.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }
    
}