using Elysium.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Items 
{
    public static class RandomItemSelection
    {
        [System.Serializable]
        public class ItemDropData
        {
            [RequireInterface(typeof(IInventoryElement))]
            public Item item;
            [Range(0, 100)] public float dropChance;
        }

        [System.Serializable]
        public class ResultData
        {
            [RequireInterface(typeof(IInventoryElement))]
            public Item item;
            public int quantity;

            public ResultData(Item item, int quantity)
            {
                this.item = item;
                this.quantity = quantity;
            }
        }

        public static List<ResultData> OpenMultiChest(List<ItemDropData> list)
        {
            List<ResultData> contents = new List<ResultData>();

            foreach (ItemDropData crateContent in list)
            {
                float r = UnityEngine.Random.Range(0f, 100f);
                if (r < crateContent.dropChance)
                {
                    contents.Add(new ResultData(crateContent.item, 1));
                }
            }

            return contents;
        }

        public static List<ResultData> OpenUniqueChest(List<ItemDropData> list)
        {
            List<ResultData> contents = new List<ResultData>();
            float totalProbability = 0;

            foreach (ItemDropData crateContent in list)
            {
                totalProbability += crateContent.dropChance;
            }

            float random = UnityEngine.Random.Range(0f, totalProbability);

            float minChance = 0;

            foreach (ItemDropData crateContent in list)
            {
                if (crateContent.dropChance == 0)
                {
                    continue;
                }

                if (random >= minChance && random < minChance + crateContent.dropChance)
                {
                    contents.Add(new ResultData(crateContent.item, 1));
                    return contents;
                }

                minChance += crateContent.dropChance;
            }

            throw new System.Exception("NO VALID ITEMS FOUND!");
        }
    }
}