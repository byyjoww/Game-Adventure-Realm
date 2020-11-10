using Elysium.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    string ItemName { get; }
    Sprite ItemSprite { get; }
    Armory.SlotType Slot { get; }
    CharacterStatWrapper[] EquipmentStats { get; }
    CharacterParameterWrapper[] EquipmentParameters { get; }
}