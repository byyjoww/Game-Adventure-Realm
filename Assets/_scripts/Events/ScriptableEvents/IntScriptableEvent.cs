using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewIntEvent", menuName = "Scriptable Objects/Scriptable Events/Primitive/Int Event", order = 1)]
public class IntScriptableEvent : GenericScriptableEvent<int>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField]
    int editorData;
    
    [CustomEditor(typeof(IntScriptableEvent), true)]
    public class GameEventEditor : Editor
    {   
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as IntScriptableEvent);
                e.Raise(e.editorData);
            }

            if (GUILayout.Button("RequestRaise"))
            {
                var e = (target as IntScriptableEvent);
                e.RequestRaise();
            }
        }
    }
#endif
}
