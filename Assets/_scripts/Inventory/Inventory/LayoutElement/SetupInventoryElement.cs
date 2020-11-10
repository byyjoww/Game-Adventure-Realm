using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SetupInventoryElement : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextComponent;
    [SerializeField] TMP_Text countTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;
    [SerializeField] GameObject pfDetailsPanel;
    [SerializeField] GameObject countPanel;

    private bool detailsWindowOpen = false;
    public event Action OnWindowClose;

    public void Setup(InventoryStack stack, Action action)
    {
        if (stack == null) { SetupEmptyElement(); return; }

        if (nameTextComponent != null) { nameTextComponent.text = stack.Item.ItemName; }

        if (countPanel != null)
        {
            countPanel.SetActive(false);
            if (countTextComponent != null && stack.Item.IsStackable)
            {
                countTextComponent.text = stack.amount.ToString();
                countPanel.SetActive(true);
            }
        }

        if (imageComponent != null) { imageComponent.sprite = stack.Item.ItemSprite; }

        if (buttonComponent == null || pfDetailsPanel == null) { Debug.LogError("button component or details panel isnt set"); return; }

        buttonComponent.onClick.AddListener(delegate { ButtonAction(stack, action); });

        OnWindowClose += () => { detailsWindowOpen = false; };
    }

    private void SetupEmptyElement()
    {
        if (countPanel != null) { countPanel.SetActive(false); }
        if (buttonComponent != null) { buttonComponent.interactable = false; }
        return;
    }

    public void ButtonAction(InventoryStack stack, Action action)
    {
        if(detailsWindowOpen) { return; }
        detailsWindowOpen = true;
        var obj = Instantiate(pfDetailsPanel, transform.root);
        obj.GetComponent<SetupDetailsPanel>().Setup(stack.Item, action, OnWindowClose);
    }

    public void AdjustCount(int count)
    {
        if (countTextComponent.gameObject.activeSelf == false)
        {
            countTextComponent.gameObject.SetActive(true);
        }

        countTextComponent.text = count.ToString();
    }
}
