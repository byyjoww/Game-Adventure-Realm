using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TabButton : MonoBehaviour
{
    [SerializeField] private TabManager tabManager;
    [SerializeField] private Button button;
    [SerializeField] private GameObject panel;
    [SerializeField] private bool isDefault;

    private void Start()
    {
        button.onClick.AddListener(Select);

        if (isDefault)
        {
            Select();
        }
    }

    public void Select()
    {
        tabManager.OnButtonSelected.Raise();
        button.interactable = false;
        tabManager.OnTabSelected(this);
        if (panel != null)
        {
            panel.SetActive(true);
        }
    }

    public void Deselect()
    {
        tabManager.OnButtonDeselected.Raise();
        button.interactable = true;
        if (panel != null)
        {
            panel.SetActive(false);
        }        
    }

    private void OnValidate()
    {
        if (button == null) button = GetComponent<Button>();
    }
}
