using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AssetContainerEditor<T> : Editor where T : ScriptableObject
{
    //SerializedProperty Nodes;
    List<bool> fold;
    Dictionary<T, Editor> editor;

    private void OnEnable()
    {
        //Nodes = serializedObject.FindProperty("Nodes");
        fold = new List<bool>();
        editor = new Dictionary<T, Editor>();
    }

    public override void OnInspectorGUI()
    {
        // If needs to Serialize other attributes
        base.OnInspectorGUI(); // should be equivalent to DrawDefaultInspector();

        // Get the instance that is using the editor
        AssetContainer<T> container = (AssetContainer<T>)target;

        serializedObject.Update();

        // Shows an array of Asset References:
        //EditorGUILayout.PropertyField(Nodes, true);
        // Or, equivalently
        //foreach (SerializedProperty p in Nodes)
        //    EditorGUILayout.PropertyField(p);


        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Components: "+container.Nodes.Count, EditorStyles.boldLabel);
        if (GUILayout.Button("Refresh"))
            LoadAssets(true);
        if (GUILayout.Button("Clear"))
            ClearAssets();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Calls the object's OnGUI
        //(serializedObject.targetObject as Container).OnGUI();


        for (int i = 0; i < container.Nodes.Count; i++)
        {
            T node = container.Nodes[i];

            // We add a label and a 'remove' button for each scriptable object
            GUILayout.BeginHorizontal();

            if (fold.Count <= i) fold.Add(new bool());

            string title = container.Nodes[i].GetType().ToString();
            fold[i] = EditorGUILayout.Foldout(fold[i], title, true);
            
            if (GUILayout.Button("X", GUILayout.MaxWidth(20f)))
            {
                RemoveAssetAt(i);
                i--;
                continue;
            }
            GUILayout.EndHorizontal();

            if (fold[i])
            {
                // Create a sub-inspector for each Scriptable Object

                if (!editor.ContainsKey(node))
                {
                    editor.Add(node, Editor.CreateEditor(container.Nodes[i]));
                }

                editor[node].OnInspectorGUI();

                EditorGUILayout.Space();
            }
        }

        DrawButtons(container);

        serializedObject.ApplyModifiedProperties();

        // if (GUI.changed) EditorUtility.SetDirty(item); item <- Container instance
    }

    int index = 0;
    protected void DrawButtons(AssetContainer<T> c)
    {        
        if (GUILayout.Button("Add Component"))
        {
            AddNewAsset(Children[index]);
        }

        string[] options = Children.Select(s => s.ToString()).ToArray();
        index = EditorGUILayout.Popup(index, options);
    }

    protected void OnValidate()
    {
        AssetContainer<T> container = (AssetContainer<T>)target;

        container.OnValidate();
    }

    public List<Type> Children { get { return typeof(T).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(T))).ToList(); } } // || type.Equals(typeof(T))

    public void LoadAssets(bool cleanOthers = false)
    {
        AssetContainer<T> container = (AssetContainer<T>)target;

        var assets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(container));

        if (cleanOthers)
        {
            foreach (var asset in assets)
                if (!(asset is T) && asset != container)
                {
                    Debug.Log("Asset Destroyed");
                    DestroyImmediate(asset, true);
                }
            RefreshAssets();
        }

        container.Nodes = assets.Where(x => x is T && x != container).Select(x => x as T).ToList();
    }

    public void AddNewAsset(Type assetType)
    {
        AssetContainer<T> container = (AssetContainer<T>)target;

        T newAsset = (T)CreateInstance(assetType);
        //newAsset.hideFlags = HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(newAsset, container); //this->AssetContainer

        // Reimport the asset after adding an object.
        // Otherwise the change only shows up when saving the project
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newAsset));
        AssetDatabase.SaveAssets(); //new
        AssetDatabase.Refresh(); // new

        container.Nodes.Add(newAsset);
    }

    public void RemoveAssetAt(int i)
    {
        AssetContainer<T> container = (AssetContainer<T>)target;

        T asset = container.Nodes[i];
        container.Nodes.RemoveAt(i);
        DestroyImmediate(asset, true);
        RefreshAssets();
    }

    public void ClearAssets()
    {
        AssetContainer<T> container = (AssetContainer<T>)target;

        for (int i = container.Nodes.Count - 1; i >= 0; i--)
            RemoveAssetAt(i);
    }

    public void RefreshAssets()
    {
        AssetContainer<T> container = (AssetContainer<T>)target;

        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(container));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
