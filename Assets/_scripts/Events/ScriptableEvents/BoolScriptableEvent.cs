using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewBoolEvent", menuName = "Scriptable Objects/Scriptable Events/Primitive/Bool Event", order = 1)]
public class BoolScriptableEvent : GenericScriptableEvent<bool>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField]
    bool editorData;
    
    [CustomEditor(typeof(BoolScriptableEvent), true)]
    public class GameEventEditor : Editor
    {   
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as BoolScriptableEvent);
                e.Raise(e.editorData);
            }
        }
    }
#endif
}
