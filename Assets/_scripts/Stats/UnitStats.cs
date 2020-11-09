using Elysium.Items;
using Elysium.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class UnitStats : ScriptableObject
{
    [Header("Stats")]
    public CharacterStatWrapper[] Stats = new CharacterStatWrapper[6];

    [Header("Loot")]
    [SerializeField] private int experience;
    [SerializeField] private int gold;
    [SerializeField] private List<RandomItemSelection.ItemDropData> PossibleItemsList;

    public LootDetails Loot => new LootDetails(experience, gold, PossibleItemsList);

    #region STAT_LIST_GENERATION
    private void OnValidate()
    {
        var numOfEnumElements = Enum.GetValues(typeof(CharacterStat.StatType)).Length;

        if (Stats.Length != numOfEnumElements) { RepopulateStatList(numOfEnumElements); }

        foreach (var stat in Stats)
        {
            if (Stats.Where(x => x.StatName == stat.StatName).Count() > 1)
            {
                RepopulateStatList(numOfEnumElements);
                break;
            }
        }
    }

    private void RepopulateStatList(int numberOfStats)
    {
        Stats = new CharacterStatWrapper[numberOfStats];

        for (int i = 0; i < Stats.Length; i++)
        {
            Stats[i] = new CharacterStatWrapper((CharacterStat.StatType)i, 1);
        }
    }
    #endregion
}