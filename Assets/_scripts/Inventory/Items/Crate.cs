using Elysium.Items;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Crate", menuName = "Scriptable Objects/Item/Crate")]
public class Crate : ScriptableObject
{
    [SerializeField] private bool givesUnique;
    [SerializeField] private List<RandomItemSelection.ItemDropData> possibleItemsList = new List<RandomItemSelection.ItemDropData>();
    
    public List<RandomItemSelection.ResultData> CrateContents 
    { 
        get 
        { 
            if (givesUnique) 
            {
                return RandomItemSelection.OpenUniqueChest(possibleItemsList);
            }
            else
            {
                return RandomItemSelection.OpenMultiChest(possibleItemsList);
            }
        } 
    }
}