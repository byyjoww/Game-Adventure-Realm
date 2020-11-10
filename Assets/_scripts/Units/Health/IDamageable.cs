using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    bool IsDead { get; }
    DamageTeam Team { get; }
    GameObject DamageableObject { get; }
    event Action OnDeathStatusChange;
    void TakeDamage(IDamageDealer damageComponent, int damage);
    void Heal(IDamageDealer damageComponent, int damage);
}
