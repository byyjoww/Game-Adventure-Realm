using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTeam { CHARACTER = 0, ENEMY = 1, }
public interface IDamageDealer
{
    int Damage { get; }
    List<DamageTeam> DealsDamageTo { get; }
    GameObject DamageDealerObject { get; }
}
