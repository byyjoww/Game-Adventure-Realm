using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SetupDetailsPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameTextComponent;
    [SerializeField] TMP_Text descriptionTextComponent;
    [SerializeField] Image imageComponent;
    [SerializeField] Button buttonComponent;

    public event Action OnWindowClose;

    public void Setup(IInventoryElement inventoryObject, Action action, Action windowCloseCallback)
    {
        nameTextComponent.text = inventoryObject.ItemName;
        descriptionTextComponent.text = inventoryObject.ItemDescription;
        imageComponent.sprite = inventoryObject.ItemSprite;
        OnWindowClose += windowCloseCallback;
        if (action != null) { buttonComponent.onClick.AddListener(delegate { ButtonAction(inventoryObject, action); }); }
        else { buttonComponent.gameObject.SetActive(false); }
    }

    public void ButtonAction(IInventoryElement inventoryObject, Action action)
    {
        action?.Invoke();
    }

    private void OnDestroy()
    {
        OnWindowClose?.Invoke();
    }
}