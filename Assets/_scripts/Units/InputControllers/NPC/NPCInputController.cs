using UnityEngine;
using UnityEngine.AI;
using Elysium.AI.Senses;
using Elysium.Attributes;
using Elysium.Timers;

public class NPCInputController : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private UnitDetection detection;
    private AnimationController anim;
    private AttackController attack;
    private HealthController health;
    private UnitMovementController movement;
    private SmartAnimal smartAnimal;
    private RuntimeStats stats;

    [ReadOnly] public string currentState;

    [SerializeField] private float respawnTime = 100f;
    [SerializeField] private float dissapearTime = 20f;

    private void Awake() => GetComponents();

    private void GetComponents()
    {
        movement = GetComponent<UnitMovementController>();
        attack = GetComponent<AttackController>();
        health = GetComponentInChildren<HealthController>();
        navAgent = GetComponent<NavMeshAgent>();
        detection = GetComponent<UnitDetection>();
        anim = GetComponent<AnimationController>();
        smartAnimal = GetComponent<SmartAnimal>();
        stats = GetComponent<RuntimeStats>();
    }

    private void Start()
    {
        TransitionToState("idle");

        detection.OnTargetDetected += () => 
        {
            SetTarget(detection.Targets[0]); 
        };

        health.OnTakeDamage += (damageDealer, damage) => 
        {
            if (!health.IsDead)
            {
                SetTarget(damageDealer.DamageDealerObject.transform);
            }
        };

        detection.OnThreatListUpdated += () =>
        {
            CheckTarget(movement.target);
        };
    }

    private void SetTarget(Transform _target)
    {
        movement.target = _target;
        attack.target = _target;

        if (_target == null) { attack.combatTarget = null; return; }
        attack.combatTarget = _target.GetComponentInChildren<IDamageable>();        
    }

    private void CheckTarget(Transform _target)
    {
        if (_target == null) { return; }
        if (Vector3.Distance(transform.position, _target.position) > (detection.ViewSphereRadius * 2))
        {
            movement.target = null;
            attack.target = null;
            attack.combatTarget = null;
        }
    }

    public void Die() => TransitionToState("dead");
    public void Respawn() => TransitionToState("respawn");

    private void Update()
    {
        if (currentState == "dead") { return; }

        // STATES
        if (attack.combatTarget == null)
        {
            // WANDER
            if (!attack.IsAttacking) { if (currentState != "wander") { TransitionToState("wander"); } }
        }
        else if (attack.combatTarget != null)
        {
            // HAS ENEMY TARGET
            if (Vector3.Distance(transform.position, movement.target.position) > attack.AttackRange)
            {
                // NOT IN RANGE
                if (!attack.IsAttacking) { if (currentState != "chase") { TransitionToState("chase"); } }                
            }
            else if (attack.CanAttack)
            {
                // IN RANGE & CAN ATTACK
                if (currentState != "attack") { TransitionToState("attack"); }
            }
            else
            {
                // IN RANGE BUT CANT ATTACK
                if (!attack.IsAttacking) { if (currentState != "idle") { TransitionToState("idle"); } }
            }
        }
        
        if (currentState == "dead") { UpdateDead(); }
        else if (currentState == "respawn") { UpdateRespawn(); }
        else if (currentState == "idle") { UpdateIdle(); }        
        else if (currentState == "wander") { UpdateWander(); }
        else if (currentState == "chase") { UpdateChase(); }
        else if (currentState == "attack") { UpdateAttack(); }
    }

    private void TransitionToState(string state) 
    {
        if (currentState == "dead") 
        {
            if (state == "respawn") { ExitDead(); } 
            else { return; } 
        }
        else if (currentState == "respawn") { ExitRespawn(); }
        else if (currentState == "idle") { ExitIdle(); }
        else if (currentState == "wander") { ExitWander(); }
        else if (currentState == "chase") { ExitChase(); }
        else if (currentState == "attack") { ExitAttack(); }

        if (state == "dead") { EnterDead(); }
        else if (state == "respawn") { EnterRespawn(); }
        else if (state == "idle") { EnterIdle(); }
        else if (state == "wander") { EnterWander(); }
        else if (state == "chase") { EnterChase(); }
        else if (state == "attack") { EnterAttack(); }
    }

    private void EnterIdle() { currentState = "idle"; anim.SetAnimation(null); }
    private void UpdateIdle() {  }
    private void ExitIdle() { }

    private void EnterWander() { currentState = "wander"; }
    private void UpdateWander() { movement.Wander(navAgent); }
    private void ExitWander() { }

    private void EnterChase() { currentState = "chase"; }
    private void UpdateChase() { movement.MoveToTarget(navAgent); }
    private void ExitChase() { }

    private void EnterAttack() { currentState = "attack"; navAgent.isStopped = true; }
    private void UpdateAttack() { transform.LookAt(movement.target); attack.TryAttack(1.7f); }
    private void ExitAttack() { }

    private void EnterDead()
    { 
        currentState = "dead"; 
        anim.SetAnimation("isDead");
        navAgent.isStopped = true;
        var player = health.GetKiller().DamageDealerObject.GetComponent<PlayerRuntimeStats>();
        player.Progress.GainExperience(stats.Loot.Experience);
        smartAnimal.Die();

        Timer.CreateTimer(dissapearTime).OnTimerEnd += () =>
        {
            gameObject.SetActive(false);
            Timer.CreateTimer(respawnTime).OnTimerEnd += Respawn;
        };
    }
    private void UpdateDead() { }
    private void ExitDead() { }

    private void EnterRespawn()
    { 
        currentState = "respawn"; 
        transform.position = movement.Origin;
        SetTarget(null);
        gameObject.SetActive(true);
        health.Ressurect();
        smartAnimal.Ressurect();
    }
    private void UpdateRespawn() { }
    private void ExitRespawn() { }
}
