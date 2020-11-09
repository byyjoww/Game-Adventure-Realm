using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] protected Inventory Inventory;
    [SerializeField] protected Armory Armory;

    [SerializeField] protected GameObject pfInventoryElement;
    [SerializeField] protected GameObject pfEmptySlotElement;
    [SerializeField] protected Transform tInventoryElement;
    protected List<GameObject> objectList = new List<GameObject>();

    [SerializeField] protected int minInventoryElements = 20;
    [SerializeField] protected int minLastRowElements = 4;

    protected bool initialized = false;

    public void Init()
    {
        Inventory.OnInventoryChanged += RefreshUI;
        initialized = true;
    }

    public void OnEnable()
    {
        if (!initialized) { Init(); }        
        RefreshUI();        
    }

    protected void OnDisable()
    {
        Inventory.OnInventoryChanged -= RefreshUI;        
        initialized = false;
    }

    protected void RefreshUI()
    {
        ClearOldElements();

        foreach (var element in Inventory.AllInventoryItems)
        {
            InstantiateInventoryElements(element);
        }       

        InstantiateEmptyElements();
    }

    protected virtual void InstantiateInventoryElements(InventoryStack stack)
    {
        var obj = Instantiate(pfInventoryElement, tInventoryElement);
        Action action = null;
        if (stack != null && stack.Item is IEquipable) { action = () => EquipItem(stack.Item as IEquipable); }
        obj.GetComponent<SetupInventoryElement>().Setup(stack, action);
        objectList.Add(obj);
    }

    protected void InstantiateEmptyElements()
    {
        while (objectList.Count < minInventoryElements)
        {
            InstantiateInventoryElements(null);
        }

        if (minLastRowElements == 0) { return; }

        while (objectList.Count % minLastRowElements != 0)
        {
            InstantiateInventoryElements(null);
        }
    }

    protected void ClearOldElements()
    {
        foreach (GameObject obj in objectList)
        {
            Destroy(obj);
        }

        objectList.Clear();
    }

    protected virtual void EquipItem(IEquipable equip)
    {        
        Inventory.LoseItem(equip as IInventoryElement, 1);
        Armory.Equip(equip);
    }    
}