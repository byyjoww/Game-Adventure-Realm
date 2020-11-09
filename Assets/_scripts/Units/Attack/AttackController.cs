using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elysium.AI.Senses;
using Elysium.AI.Navmesh;
using System;
using Elysium.Attributes;
using UnityStandardAssets.Characters.FirstPerson;

public class AttackController : MonoBehaviour, IDamageDealer
{
    private AnimationController anim;
    public IDamageable combatTarget { get; set; }
    public Transform target { get; set; }
    public bool IsAttacking { get; private set; }

    public int Damage
    {
        get
        {
            SetDamage();
            return damage;
        }
    }
    private Func<float> getDamage = () => 0;
    [ReadOnly, SerializeField] int damage = 0;

    public List<DamageTeam> DealsDamageTo => dealsDamageTo;
    [SerializeField] private List<DamageTeam> dealsDamageTo;

    public GameObject DamageDealerObject => gameObject;

    [SerializeField, Range(1f, 10f)] private float attackRange = 5f;
    public float AttackRange => attackRange;
    [SerializeField, Range(1f, 10f)] private float attackInterval = 1f;
    public float AttackInterval => attackInterval;

    private float attackCooldown = 0;
    [SerializeField] private bool disabled = false;

    public bool CanAttack => attackCooldown == 0;

    private void Awake()
    {
        if (anim == null) anim = GetComponent<AnimationController>();
        IsAttacking = false;
    }

    private int SetDamage() => damage = (int)getDamage();
    public void BindDamage(Func<float> lambda) => getDamage = lambda;

    public bool TryAttack(float damageDelay)
    {
        if (disabled) { return false; }
        if (!CanAttack) { return false; }
        if (!MouseLook.GetMouseLockStatus()) { return false; }

        StartCoroutine(StartAttack(damageDelay));
        return true;
    }

    private IEnumerator StartAttack(float damageDelay)
    {
        anim.SetAnimation("isAttacking");
        IsAttacking = true;

        attackCooldown = attackInterval;
        yield return new WaitForSeconds(damageDelay);

        if (target != null && combatTarget != null)
        {
            if (Vector3.Distance(transform.position, target.position) <= attackRange && AINavigation.InTargetFieldOfView(target, transform, 40))
            {
                combatTarget.TakeDamage(this, Damage);
                //GameAssets.Instance.cameraShake.Shake(0.1f, 0.015f);
            }
        }

        anim.SetAnimation(null);
        IsAttacking = false;

        yield return null;
    }

    private void Update()
    {
        if (attackCooldown == 0) { return;}

        attackCooldown -= Time.deltaTime;
        //Debug.LogError("Attack Cooldown: " + attackCooldown);

        if (attackCooldown <= 0)
        {
            attackCooldown = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}