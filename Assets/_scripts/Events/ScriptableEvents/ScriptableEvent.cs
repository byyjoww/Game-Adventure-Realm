using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using  UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewEvent", menuName = "Scriptable Objects/Scriptable Events/Action Event", order = 1)]
public class ScriptableEvent : ScriptableObject
{
    
#pragma warning disable 0067
    public event Action OnRaise;
#pragma warning restore 0067

    public void Raise()
    {
        // Debug.LogError($"{name} Scriptable Event Raised");
        OnRaise?.Invoke();
    }
    
    
#if UNITY_EDITOR
    [CustomEditor(typeof(ScriptableEvent), true)]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Raise"))
            {
                (target as ScriptableEvent).Raise();
            }
        }
    }
#endif
    
}
