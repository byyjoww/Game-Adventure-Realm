using Elysium.Stats;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Item/Equipment")]
public class Equipment : Item, IEquipable
{    
    public Armory.SlotType Slot => slot;
    [SerializeField] private Armory.SlotType slot;

    public CharacterStatWrapper[] EquipmentStats => equipmentStats;
    [SerializeField] private CharacterStatWrapper[] equipmentStats;

    public CharacterParameterWrapper[] EquipmentParameters => equipmentParameters;
    [SerializeField] private CharacterParameterWrapper[] equipmentParameters;

    #region VALIDATION
    private void OnValidate()
    {
        ValidateStats();
        ValidateAttributes();
    }

    private void ValidateStats()
    {
        var rawValues = Enum.GetValues(typeof(CharacterStat.StatType));
        var enumValues = Array.CreateInstance(typeof(CharacterStat.StatType), rawValues.Length) as CharacterStat.StatType[];
        Array.Copy(rawValues, enumValues, rawValues.Length);

        if (equipmentStats == null) { ValidateStatsList(enumValues); return; }
        if (equipmentStats.Length != enumValues.Length) { ValidateStatsList(enumValues); return; }

        for (int i = 0; i < equipmentStats.Length; i++)
        {
            if (equipmentStats.Where(x => x.StatName == equipmentStats[i].StatName).Count() > 1) { ValidateStatsList(enumValues); return; }
        }
    }

    private void ValidateStatsList(CharacterStat.StatType[] enumValues)
    {
        equipmentStats = new CharacterStatWrapper[enumValues.Length];
        Debug.LogError("less stat slots than stat types");

        for (int i = 0; i < equipmentStats.Length; i++)
        {
            equipmentStats[i] = new CharacterStatWrapper(enumValues[i], 0);
        }
    }

    private void ValidateAttributes()
    {
        var rawValues = Enum.GetValues(typeof(CharacterParameter.ParameterType));
        var enumValues = Array.CreateInstance(typeof(CharacterParameter.ParameterType), rawValues.Length) as CharacterParameter.ParameterType[];
        Array.Copy(rawValues, enumValues, rawValues.Length);

        if (equipmentParameters == null) { ValidateAttributeList(enumValues); return; }
        if (equipmentParameters.Length != enumValues.Length) { ValidateAttributeList(enumValues); return; }

        for (int i = 0; i < equipmentParameters.Length; i++)
        {
            if (equipmentParameters.Where(x => x.AttributeName == equipmentParameters[i].AttributeName).Count() > 1) { ValidateAttributeList(enumValues); return; }
        }
    }

    private void ValidateAttributeList(CharacterParameter.ParameterType[] enumValues)
    {
        equipmentParameters = new CharacterParameterWrapper[enumValues.Length];
        Debug.LogError("less stat slots than stat types");

        for (int i = 0; i < equipmentParameters.Length; i++)
        {
            equipmentParameters[i] = new CharacterParameterWrapper(enumValues[i], 0);
        }
    }
    #endregion
}