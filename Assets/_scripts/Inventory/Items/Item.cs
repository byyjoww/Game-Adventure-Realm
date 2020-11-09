using Elysium.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Scriptable Objects/Item/Item")]
public class Item : ScriptableObject, IInventoryElement
{
    [Header("Item Details")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private bool isStackable;
    [SerializeField] private ItemRarity itemRarity;
    [SerializeField] private ItemType itemType;
    [SerializeField] private int itemValue;

    public string ItemName => itemName;
    public Sprite ItemSprite => itemSprite;
    public string ItemDescription => itemDescription;
    public bool IsStackable => isStackable;
    public ItemRarity ItemRarity => itemRarity;
    public ItemType ItemType => itemType;
    public int ItemValue => itemValue;
}
