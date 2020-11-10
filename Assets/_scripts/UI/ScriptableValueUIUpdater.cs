using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

[RequireComponent(typeof(TMP_Text))]
public class ScriptableValueUIUpdater : MonoBehaviour
{
    [SerializeField] private ScriptableValue scriptableValue;
    [SerializeField] private ScriptableEvent scriptableValueUpdateEvent;
    [SerializeField] private TMP_Text textComponent;

    [Header("Optional")]
    [SerializeField] private string prefix;
    [SerializeField] private string suffix;

    private void Start()
    {
        scriptableValueUpdateEvent.OnRaise += UpdateValues;
        UpdateValues();
    }

    private void UpdateValues()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append(prefix);
        builder.Append(scriptableValue.ValueAsString);
        builder.Append(suffix);

        textComponent.text = builder.ToString();
    }

    private void OnValidate()
    {
        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
    }
}
