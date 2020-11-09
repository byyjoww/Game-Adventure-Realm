using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private ScriptableValue uiText;

    private void Awake()
    {
        uiText.OnValueChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        textComponent.text = uiText.ValueAsString;
    }

    private void OnValidate()
    {
        if (textComponent == null) textComponent = GetComponent<TMP_Text>();
    }
}
