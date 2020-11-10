using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemRarity
{
    public enum RarityLevel { Common = 0, Uncommon = 1, Rare = 2, Epic = 3, Legendary = 4 }
    public RarityLevel itemRarityLevel;
    public Color RarityTextColor => rarityColors[itemRarityLevel];

    private Dictionary<RarityLevel, Color> rarityColors = new Dictionary<RarityLevel, Color>()
    {
        { RarityLevel.Common, Color.grey },
        { RarityLevel.Uncommon, Color.green },
        { RarityLevel.Rare, Color.blue },
        { RarityLevel.Epic, Color.magenta },
        { RarityLevel.Legendary, Color.yellow },
    };
}
