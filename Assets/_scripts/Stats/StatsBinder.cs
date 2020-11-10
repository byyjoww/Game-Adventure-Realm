using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsBinder : MonoBehaviour
{
    private RuntimeStats stats;

    // Binding Components
    private HealthController health;
    private AttackController attack;

    private void Awake()
    {
        stats = GetComponent<RuntimeStats>();
        health = GetComponentInChildren<HealthController>();
        attack = GetComponentInChildren<AttackController>();
    }

    private void Start() => StartCoroutine(BindAllEvents());

    private IEnumerator BindAllEvents()
    {
        yield return new WaitUntil(() => stats.Initialized);

        BindMaxHealth();
        BindDamage();

        yield return null;
    }

    private void BindMaxHealth() => health.BindMaxHealth(() => stats.Health);
    private void BindDamage() => attack.BindDamage(() => stats.Damage);
}
