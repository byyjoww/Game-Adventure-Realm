using Elysium.AI.Senses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTargetBinder : MonoBehaviour
{
    private UnitDetection monsterDetection;
    private AttackController attack;

    private NPCInputController unitTarget;

    public GameObject crosshair = null;

    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        attack = GetComponent<AttackController>();
        monsterDetection = GetComponent<MonsterDetection>();
    }

    private void Start()
    {
        monsterDetection.OnTargetDetected += SetTarget;
        monsterDetection.OnThreatListUpdated += CheckTarget;
    }
    private void SetTarget()
    {
        attack.target = monsterDetection.Targets[0];
        attack.combatTarget = attack.target.GetComponentInChildren<IDamageable>();
        unitTarget = attack.target.GetComponent<NPCInputController>();

        // Get Crosshair
        crosshair = attack.target.GetComponent<UI_UnitCanvas>().crosshairCanvas.gameObject;

        if (unitTarget.currentState == "dead") { return; }
        crosshair.SetActive(true);
    }

    private void CheckTarget()
    {
        if (monsterDetection.Targets.Contains(attack.target)) 
        { 
            if(unitTarget.currentState != "dead") { return; }
        }
        else 
        {
            if (attack.target != null || attack.combatTarget != null) 
            {
                attack.target = null;
                attack.combatTarget = null;
            }            

            if (crosshair == null) { return; }
            crosshair.SetActive(false); 
        }
    }
}
