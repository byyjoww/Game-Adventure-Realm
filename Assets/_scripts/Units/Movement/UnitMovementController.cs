using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Elysium.AI.Navmesh;

public class UnitMovementController : MonoBehaviour, IMovement
{
    [SerializeField, Range(1f, 50f)] private float movementSpeed = 5f;
    [SerializeField, Range(1f, 50f)] private float runningSpeed = 10f;
    [SerializeField, Range(1f, 50f)] private float wanderRadius = 10f;
    [SerializeField, Range(1f, 50f)] private float wanderInterval = 3f;

    private AnimationController anim;
    public Vector3 Origin { get; private set; }

    public Transform target { get; set; }
    private float wanderCooldown = 0;
    private bool isWandering;
    private bool disabled;

    private void Awake()
    {
        Origin = transform.position;
        anim = GetComponent<AnimationController>();
    }

    public bool CanWander => wanderCooldown == 0 && isWandering == false;

    public void MoveToTarget(NavMeshAgent navAgent)
    {
        anim.SetAnimation("isRunning");
        navAgent.speed = runningSpeed;
        navAgent.SetDestination(target.position);        
        navAgent.isStopped = false;
    }

    public void Wander(NavMeshAgent navAgent)
    {
        if (disabled) 
        { 
            navAgent.isStopped = true;
            isWandering = false;  
            return; 
        }

        if (isWandering)
        {
            if (Vector3.Distance(transform.position, navAgent.destination) < navAgent.stoppingDistance + 0.5)
            {
                // ARRIVED AT LOCATION
                anim.SetAnimation(null);
                isWandering = false;
                wanderCooldown = wanderInterval;
            }
        }        

        // IS NOT WANDERING
        if (!CanWander) { return; }

        // IS STILL WANDERING
        anim.SetAnimation("isWalking");
        var pos = AINavigation.RandomNavSphere(Origin, wanderRadius);
        navAgent.speed = movementSpeed;
        navAgent.SetDestination(pos);        
        navAgent.isStopped = false;
        isWandering = true;
    }

    private void Update()
    {
        wanderCooldown -= Time.deltaTime;
        if (wanderCooldown <= 0)
        {
            wanderCooldown = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }

    public void RestrictMovement(bool _status) => disabled = _status;
}
