using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewVector3Event", menuName = "Scriptable Objects/Scriptable Events/Complex/Vector3 Event", order = 1)]
public class Vector3ScriptableEvent : GenericScriptableEvent<Vector3>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField]
    Vector3 editorData;
    
    [CustomEditor(typeof(Vector3ScriptableEvent), true)]
    public class GameEventEditor : Editor
    {   
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as Vector3ScriptableEvent);
                e.Raise(e.editorData);
            }
        }
    }
#endif
}
