using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TabManager : MonoBehaviour
{
    public ScriptableEvent OnButtonSelected;
    public ScriptableEvent OnButtonDeselected;

    [SerializeField] private List<TabButton> tabButtons = new List<TabButton>();
    private TabButton selectedTab;

    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetAllButtons();
    }

    private void ResetAllButtons()
    {
        foreach (var button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.Deselect();
        }
    }

    private void OnValidate()
    {
        tabButtons = GetComponentsInChildren<TabButton>().ToList();
    }
}
