using Elysium.Items;
using System.Collections.Generic;

[System.Serializable]
public class LootDetails
{
    public LootDetails(int exp, int gold, List<RandomItemSelection.ItemDropData> items)
    {
        this.Experience = exp;
        this.Gold = gold;

        if (items == null) this.Items = new List<RandomItemSelection.ItemDropData>();
        else { this.Items = items; }
    }

    public int Experience { get; private set; }
    public int Gold { get; private set; }
    public List<RandomItemSelection.ItemDropData> Items { get; private set; }
}