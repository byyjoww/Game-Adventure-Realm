using Elysium.Attributes;
using Elysium.Timers;
using PolyPerfect;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAnimal : SmartObject
{
    private NPCRuntimeStats npcStats;

    private LootDetails loot => npcStats.Loot;

    [SerializeField] private GameObject interactablePanel;

    private bool isLooted = false;

    private void Awake() => npcStats = GetComponent<NPCRuntimeStats>();
    private void Start() 
    {
        OnFocusSet += () => ToggleInteractablePanel(true);
        OnFocusRemoved += () => ToggleInteractablePanel(false);
    }

    public override void Interact(SmartController smartController)
    {
        GetLoot(smartController);

        interactable = false;
        focusable = false;
    }

    public void ToggleInteractablePanel(bool _interactible) =>  interactablePanel.SetActive(_interactible);

    private void GetLoot(SmartController smartController)
    {
        if (isLooted) { Debug.Log("Already looted."); return; }

        foreach (var item in loot.Items)
        {
            float r = Random.Range(0f, 100f);
            if (r <= item.dropChance) 
            {
                smartController.Inventory.GainItem(item.item, 1);
                Debug.Log($"You looted {gameObject.name} and received 1x {item.item.ItemName}.");
            }
        }
        
        isLooted = true;
    }

    public void Die() 
    {
        Timer.CreateTimer(2f).OnTimerEnd += () =>
        {
            focusable = true;
            interactable = true;
        };           
    }

    public void Ressurect()
    {
        focusable = false;
        interactable = false;
    }
}
