﻿using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ShortcutData.Binding))]
public class ShortcutPropertyDrawer : PropertyDrawer
{
    private static readonly int ShorcutScannerHash = "ShorcutScanner".GetHashCode();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        //EditorGUI.PropertyField(position, property, label, true);
        ShorcutScanner(position, property, label);
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    private void ShorcutScanner(Rect position, SerializedProperty property, GUIContent label)
    {
        //Generate a control ID
        int controlID = GUIUtility.GetControlID(ShorcutScannerHash, FocusType.Keyboard, position);

        //Create a state that holds information about the current state of the control
        //that would otherwise be lost during a repaint event.
        KeyScannerInfoState state = (KeyScannerInfoState)GUIUtility.GetStateObject(
            typeof(KeyScannerInfoState),
            controlID);

        //Get a reference to the properties we want to modify
        SerializedProperty keyCodeProp = property.FindPropertyRelative("m_KeyCode");
        SerializedProperty modifierProp = property.FindPropertyRelative("m_Modifiers");

        //Create the style for the value field.
        //This way, we can tell if we are "recording" a keyboard event.
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.padding = new RectOffset();
        style.margin = new RectOffset();

        //Change the text color of the value field when we are scanning for input
        if (state.isScanning)
        {
            style.normal.textColor = Color.red;
        }

        //Create the content for our style
        //This is what will be displayed in the value field
        GUIContent content = new GUIContent(state.ToString());

        //Draw the prefix label and use it's position to draw the value box
        position = EditorGUI.PrefixLabel(position, controlID, label);

        //The current event being processed by the IMGUI system
        var current = Event.current;
        //Gets the current event type for this specific control
        var eventType = current.GetTypeForControl(controlID);

        switch (eventType)
        {
            case EventType.MouseDown:
                if (position.Contains(current.mousePosition))
                {
                    //Check if no other controls have hotcontrol OR if we are scanning for input
                    if (GUIUtility.hotControl == 0 || state.isScanning)
                    {
                        //Set hotcontrol to our ID, so other controls know not to listen to events
                        GUIUtility.hotControl = controlID;
                        //Do the same for keyboard control
                        GUIUtility.keyboardControl = controlID;
                        //Consume this event, causing other GUI elements to ignore it.
                        current.Use();
                    }
                }

                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    //Is the cursor within the value rect?
                    if (position.Contains(current.mousePosition))
                    {
                        //Toggle the scan state
                        state.isScanning = !state.isScanning;
                    }
                    else
                    {
                        //If we click somewhere else, disable recording
                        state.isScanning = false;
                    }

                    //If we are no longer scanning, give back the controls
                    if (!state.isScanning)
                    {
                        //release hot control so other controls can use it now
                        GUIUtility.keyboardControl = 0;
                        GUIUtility.hotControl = 0;

                        //set the values to the properties
                        modifierProp.enumValueIndex = (int)state.modifiers;
                        keyCodeProp.enumValueIndex = (int)state.keyCode;
                        //We changed the input values, so notify the IMGUI that something changed
                        GUI.changed = true;
                    }

                    //Consume the event
                    current.Use();
                }

                break;
            case EventType.KeyDown:
                //If we are not scanning, stop here
                if (!state.isScanning) return;
                //If the current key is not a keyboard key OR the current key is none, stop here
                if (!current.isKey || current.keyCode == KeyCode.None) return;

                //Set the state properties to the pressed key and optionally event modifiers
                state.modifiers = current.modifiers;
                state.keyCode = current.keyCode;
                //Consume the event
                current.Use();

                break;
            case EventType.Repaint:
                //Draw the style that we made earlier.
                style.Draw(position, content, controlID, state.isScanning);
                break;
        }
    }

    /// <summary>
    /// This class holds on to our data every time the IMGUI gets repainted
    /// </summary>
    private class KeyScannerInfoState
    {
        public bool isScanning;
        public KeyCode keyCode;
        public EventModifiers modifiers;

        public override string ToString()
        {
            if (modifiers == EventModifiers.None)
                return keyCode.ToString();

            return string.Join("+", modifiers, keyCode);
        }

        public void ResetState()
        {
            keyCode = KeyCode.None;
            modifiers = EventModifiers.None;
        }
    }
}