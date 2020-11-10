using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewStringEvent", menuName = "Scriptable Objects/Scriptable Events/Primitive/String Event", order = 1)]
public class StringScriptableEvent : GenericScriptableEvent<string>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField]
    string editorData;
    
    [CustomEditor(typeof(StringScriptableEvent), true)]
    public class GameEventEditor : Editor
    {   
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as StringScriptableEvent);
                e.Raise(e.editorData);
            }
        }
    }
#endif
}
