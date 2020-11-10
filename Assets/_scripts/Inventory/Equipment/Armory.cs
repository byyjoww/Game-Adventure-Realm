using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Elysium.Attributes;
using Elysium.Stats;

[CreateAssetMenu(fileName = "Armory", menuName = "Scriptable Objects/Item/Armory")]
public class Armory : ScriptableObject
{
    public enum SlotType { Head, Body, Legs, Shoes, Weapon, Shield }

    [System.Serializable]
    public class ArmorySlot
    {
        public ArmorySlot(SlotType slotType)
        {
            this.slotType = slotType;
        }

        [SerializeField, ReadOnly, RequireInterface(typeof(IEquipable))] private UnityEngine.Object equipment;
        public IEquipable Equipment
        {
            get { if (equipment != null) { return equipment as IEquipable; } else { return defaultEquipment as IEquipable; } }
            set => equipment = (UnityEngine.Object)value;
        }
        [SerializeField, RequireInterface(typeof(IEquipable))] protected UnityEngine.Object defaultEquipment;
        public IEquipable DefaultEquipment => defaultEquipment as IEquipable;
        [ReadOnly] public SlotType slotType;
    }
        
    [SerializeField] protected Inventory inventory;
    public ArmorySlot[] ArmorySlots => armorySlots;
    [SerializeField] private ArmorySlot[] armorySlots;

    public event Action<IEquipable> OnEquipmentEquipped;
    public event Action<IEquipable> OnEquipmentUnequipped;

    private void OnEnable()
    {
        if(inventory == null) { throw new System.Exception("inventory reference not set in armory"); }
        OnEquipmentUnequipped += ReturnEquippedItem;
    }

    private void OnDisable() => OnEquipmentUnequipped -= ReturnEquippedItem;

    protected virtual void ReturnEquippedItem(IEquipable equip) => inventory.GainItem(equip as IInventoryElement, 1);

    private ArmorySlot GetSlotByType(SlotType slotType) => armorySlots.Single(x => x.slotType == slotType);

    public bool Equip(IEquipable equipment)
    {
        if (equipment == null) { return false; }

        var slot = GetSlotByType(equipment.Slot);
        Unequip(slot);
        slot.Equipment = equipment;
        OnEquipmentEquipped?.Invoke(equipment);

        return true;
    }

    public bool Unequip(ArmorySlot slot)
    {
        if (slot.Equipment == slot.DefaultEquipment) { return false; }

        var previousEquipment = slot.Equipment;
        slot.Equipment = null;
        OnEquipmentUnequipped?.Invoke(previousEquipment);

        return true;
    }

    #region ITEM_STATS
    public int GetStats(CharacterStat.StatType statType)
    {
        int value = 0;

        for (int i = 0; i < armorySlots.Length; i++)
        {
            if (armorySlots[i].Equipment == null) { continue; }
            value += armorySlots[i].Equipment.EquipmentStats.SingleOrDefault(x => x.StatName == statType).Value;
        }

        return value;
    }

    public int GetAttributes(CharacterParameter.ParameterType attributeType)
    {
        int value = 0;

        for (int i = 0; i < armorySlots.Length; i++)
        {
            value += armorySlots[i].Equipment.EquipmentParameters.SingleOrDefault(x => x.AttributeName == attributeType).Value;
        }

        return value;
    }
    #endregion

    #region VALIDATION
    private void OnValidate()
    {
        var rawValues = Enum.GetValues(typeof(SlotType));
        var enumValues = Array.CreateInstance(typeof(SlotType), rawValues.Length) as SlotType[];
        Array.Copy(rawValues, enumValues, rawValues.Length);

        if (armorySlots == null) { ValidateList(enumValues); return; }
        if (armorySlots.Length != enumValues.Length) { ValidateList(enumValues); return; }
        
        for (int i = 0; i < armorySlots.Length; i++)
        {
            if (armorySlots.Where(x => x.slotType == armorySlots[i].slotType).Count() > 1) { ValidateList(enumValues); return; }
        }
    }

    private void ValidateList(SlotType[] enumValues)
    {        
        armorySlots = new ArmorySlot[enumValues.Length];
        Debug.LogError("less equipment slots than equipment types");

        for (int i = 0; i < armorySlots.Length; i++)
        {
            armorySlots[i] = new ArmorySlot(enumValues[i]);
        }
    }
    #endregion
}