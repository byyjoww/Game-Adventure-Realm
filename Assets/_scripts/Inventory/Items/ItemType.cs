using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemType
{
    public enum AvailableTypes { Light = 0, Medium = 1, Heavy = 2 }
    public AvailableTypes itemType;
    public Color TypeTextColor => typeColors[itemType];

    private Dictionary<AvailableTypes, Color> typeColors = new Dictionary<AvailableTypes, Color>()
    {
        { AvailableTypes.Light, Color.grey },
        { AvailableTypes.Medium, Color.grey },
        { AvailableTypes.Heavy, Color.grey },
    };
}
