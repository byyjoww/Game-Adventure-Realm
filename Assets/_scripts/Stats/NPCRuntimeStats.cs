using Elysium.Items;
using System.Collections.Generic;
using UnityEngine;

public class NPCRuntimeStats : RuntimeStats
{
    public UnitStats baseStats;

    private void Start() => Init(baseStats.Stats, baseStats.Loot);
}
