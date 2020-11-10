using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewFloatEvent", menuName = "Scriptable Objects/Scriptable Events/Primitive/Float Event", order = 1)]
public class FloatScriptableEvent : GenericScriptableEvent<float>
{
#if UNITY_EDITOR

    [Header("Editor Only")]
    [SerializeField]
    float editorData;
    
    [CustomEditor(typeof(FloatScriptableEvent), true)]
    public class GameEventEditor : Editor
    {   
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                var e = (target as FloatScriptableEvent);
                e.Raise(e.editorData);
            }
        }
    }
#endif
}
