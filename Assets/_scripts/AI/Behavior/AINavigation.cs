using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

namespace Elysium.AI.Navmesh 
{
    public static class AINavigation
    {
        #region BASIC_NAVMESH_BEHAVIOR
        /// <summary>
        /// Moves directly towards the target location.
        /// </summary>
        public static void Seek(Vector3 targetLocation, NavMeshAgent agent)
        {
            agent.SetDestination(targetLocation);
        }

        /// <summary>
        /// Moves directly away from the target location. If the unit cannot create a path to target location, it will return false;
        /// </summary>
        public static Vector3 Flee(Vector3 targetLocation, NavMeshAgent agent, float fleeRadius = 10f)
        {
            Vector3 fleeVector = (agent.transform.position - targetLocation).normalized;
            Vector3 newGoal = agent.transform.position + fleeVector * fleeRadius;

            //NavMeshPath path = new NavMeshPath();
            //agent.CalculatePath(newGoal, path);

            //if (path.status != NavMeshPathStatus.PathInvalid)
            //{
            //    agent.SetDestination(path.corners[path.corners.Length - 1]);
            //    return true;
            //}

            return newGoal;
        }

        /// <summary>
        /// Predicts the target's future position based on their speed and moves towards the predicted location.
        /// </summary>
        public static void Intercept(Transform target, NavMeshAgent agent)
        {
            Vector3 targetDir = target.transform.position - agent.transform.position;
            float targetSpeed = target.GetComponent<NavMeshAgent>().speed;

            float relativeHeading = Vector3.Angle(agent.transform.forward, agent.transform.TransformVector(target.forward));
            float toTarget = Vector3.Angle(agent.transform.forward, agent.transform.TransformVector(targetDir));

            if ((toTarget > 90 && relativeHeading < 20) || targetSpeed < 0.01f)
            {
                Seek(target.transform.position, agent);
                return;
            }

            float lookAhead = targetDir.magnitude / (agent.speed + targetSpeed);
            Vector3 interceptPointLocation = target.transform.position + target.forward * lookAhead;

            Seek(interceptPointLocation, agent);
        }

        /// <summary>
        /// Predicts the target's future position based on their speed and moves away from the predicted location.
        /// </summary>
        public static void Evade(Transform target, NavMeshAgent agent)
        {
            Vector3 targetDir = target.transform.position - agent.transform.position;

            var nm = target.GetComponent<NavMeshAgent>();

            if(nm == null)
            {
                return;
            }

            float targetSpeed = nm.speed;

            float lookAhead = targetDir.magnitude / (agent.speed + targetSpeed);
            Vector3 interceptPointLocation = target.transform.position + target.forward * lookAhead;

            Flee(interceptPointLocation, agent);
        }

        /// <summary>
        /// Generates a circle area where the NPC can wander around. Must save "wanderTarget" as a local variable, so the NPC can keep track of where the last location is.
        /// </summary>
        public static Tuple<Vector3, Vector3> Wander(Vector3 wanderTarget, NavMeshAgent agent, float wanderRadius = 10f, float wanderDistance = 20f, float wanderJitter = 1f)
        {
            wanderTarget += new Vector3(UnityEngine.Random.Range(-1f, 1f) * wanderJitter, 0, UnityEngine.Random.Range(-1f, 1f) * wanderJitter);
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            Vector3 targetLocal = wanderTarget + new Vector3(0f, 0f, wanderDistance);
            Vector3 targetWorld = agent.transform.InverseTransformVector(targetLocal);

            Debug.DrawRay(agent.transform.position, targetWorld, Color.yellow);
            return new Tuple<Vector3, Vector3> (wanderTarget, targetWorld);
        }

        public static Tuple<Vector3, Vector3> Wander2(Vector3 wanderTarget, NavMeshAgent agent, float wanderRadius = 10f, float wanderDistance = 20f, float wanderJitter = 1f)
        {
            var circleCenter = agent.velocity;
            circleCenter.Normalize();
            circleCenter *= wanderDistance;

            var displacement = new Vector3(0, 0, -1);
            displacement *= wanderRadius;
            displacement = Quaternion.AngleAxis(wanderTarget.z, Vector3.up) * displacement;

            //wanderTarget.z = UnityEngine.Random.Range(wanderTarget.z * (wanderTarget.z - wanderJitter), wanderTarget.z * (wanderTarget.z + wanderJitter));

            var wanderForce = circleCenter + displacement;

            return new Tuple<Vector3, Vector3>(wanderTarget, wanderForce);
        }

        /// <summary>
        /// Returns a random location within the NavMesh, inside the unit sphere, at a max distance.
        /// </summary>
        public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask = -1, bool needsExactLocation = true)
        {
            Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist;
            randDirection += origin;
            
            if (needsExactLocation)
            {
                NavMeshPath path = new NavMeshPath();
                NavMesh.CalculatePath(origin, randDirection, layermask, path);
                return randDirection;
            }
            else
            {
                NavMeshHit navHit;
                NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
                return navHit.position;
            }
        }

        /// <summary>
        /// Determines the closest hiding spot from a target and moves behind it, based on a list of possible hiding spot obstacles.
        /// </summary>
        public static void Hide(Transform target, NavMeshAgent agent, List<Transform> hideableObstacles, float maxRaycastDistance = 100f, float objHideOffset = 3f, float minTargetDistance = 0f)
        {
            if (!CanSeeTarget(target, agent))
            {
                if (Vector3.Distance(agent.transform.position, target.position) >= minTargetDistance)
                {
                    return;
                }
            }

            if (hideableObstacles == null || hideableObstacles.Count < 1)
            {
                Debug.LogError($"No hideable obstacles in {hideableObstacles}.");
                return;
            }

            float dist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;
            Vector3 chosenDirection = Vector3.zero;
            Transform chosenHidingObj = hideableObstacles[0];

            for (int i = 0; i < hideableObstacles.Count; i++)
            {
                var targetDist = Vector3.Distance(hideableObstacles[i].position, target.position);

                if (targetDist <= minTargetDistance)
                {
                    continue;
                }

                Vector3 hideDirection = hideableObstacles[i].position - target.position;
                Vector3 hidePosition = hideableObstacles[i].position + hideDirection.normalized * 10;

                if (Vector3.Distance(agent.transform.position, hidePosition) < dist)
                {
                    chosenSpot = hidePosition;
                    chosenDirection = hideDirection;
                    chosenHidingObj = hideableObstacles[i];
                    dist = Vector3.Distance(agent.transform.position, hidePosition);
                }
            }

            Collider hideCol = chosenHidingObj.gameObject.GetComponent<Collider>();

            Ray backRay = new Ray(chosenSpot, -chosenDirection.normalized);
            Debug.DrawRay(chosenSpot, -chosenDirection.normalized, Color.red);

            RaycastHit info;
            hideCol.Raycast(backRay, out info, maxRaycastDistance);

            Seek(info.point + chosenDirection.normalized * (agent.radius + objHideOffset), agent);
        }
        #endregion

        #region NAVMESH_FOV_CHECK
        public static bool CanSeeTarget(Transform target, NavMeshAgent agent)
        {
            RaycastHit raycastInfo;
            Vector3 rayToTarget = target.transform.position - agent.transform.position;
            Debug.DrawRay(agent.transform.position, rayToTarget, Color.green);
            if (Physics.Raycast(agent.transform.position, rayToTarget, out raycastInfo))
            {
                if (raycastInfo.transform.gameObject.transform == target)
                {
                    Debug.Log("Agent is visible.");
                    return true;
                }
            }

            Debug.Log("Agent is hidden.");
            return false;
        }

        public static bool InTargetFieldOfView(Transform target, Transform agent, float fieldOfViewAngle = 60)
        {
            Vector3 targetDirection = target.position - agent.position;
            float lookingAngle = Vector3.Angle(targetDirection, agent.forward);

            if (lookingAngle < fieldOfViewAngle)
            {
                return true;
            }

            return false;
        }

        public static List<Transform> LocateTargetBlind(Transform agent, float detectionRadius, float fieldOfViewAngle)
        {
            List<Transform> targets = new List<Transform>();

            var targetsInRange = Physics.OverlapSphere(agent.position, detectionRadius);

            foreach (var unit in targetsInRange)
            {
                if(InTargetFieldOfView(unit.transform, agent, fieldOfViewAngle))
                {
                    targets.Add(unit.transform);
                }
            }
            
            return targets;
        }

        public static List<Transform> LocateTargetBlind(Transform agent, float detectionRadius)
        {
            List<Transform> targets = new List<Transform>();

            var targetsInRange = Physics.OverlapSphere(agent.position, detectionRadius);

            foreach (var unit in targetsInRange)
            {
                targets.Add(unit.transform);
            }

            return targets;
        }
        #endregion        
    }
}