using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Elysium.AI
{
    public static class AICrowdSimulation
    {
        #region FLOCKING_BEHAVIOR
        /// <summary>
        /// Create a new flock and assign the necessary behavior.
        /// </summary>
        public static void CreateFlock(FlockingData flockingData, Transform parent = null)
        {
            flockingData.FlockGoal = flockingData.FlockCenter.position;

            int numUnits = Random.Range(flockingData.minUnits, flockingData.maxUnits + 1);
            flockingData.allUnits = new UnitData[numUnits];
            for (int i = 0; i < numUnits; i++)
            {
                Vector3 pos = flockingData.FlockGoal + new Vector3(Random.Range(-flockingData.FlockLimits.x, flockingData.FlockLimits.x),
                                                                   Random.Range(-flockingData.FlockLimits.y, flockingData.FlockLimits.y),
                                                                   Random.Range(-flockingData.FlockLimits.z, flockingData.FlockLimits.z));

                pos = flockingData.FlockGoal;

                if (flockingData.unitPrefabs.Count < 1) { Debug.LogError("No unit prefabs set in flock."); return; }
                var r = Random.Range(0, flockingData.unitPrefabs.Count);
                GameObject prefab = flockingData.unitPrefabs[r];

                flockingData.allUnits[i].transform = GameObject.Instantiate(prefab, pos, Quaternion.identity).transform;
                flockingData.allUnits[i].meshRenderer = flockingData.allUnits[i].transform.GetComponentInChildren<Renderer>();
                flockingData.allUnits[i].transform.name = $"{prefab.name} {i}";
                if (parent != null) { flockingData.allUnits[i].transform.parent = parent; }                
                flockingData.allUnits[i].speed = Random.Range(flockingData.unitMinSpeed, flockingData.unitMaxSpeed);
            }
        }

        /// <summary>
        /// Generates a flocking behavior based on parameters contained within FlockingData.
        /// </summary>
        public static void Flock(FlockingData flockingData, bool checkForBounds = true, bool checkForCollision = true)
        {
            // Change goal position within flock limits based on random frequency
            if (Random.Range(0, 100) < flockingData.positionGoalChangeFrequency)
            {
                flockingData.AdjustGoalWithinFlockRange(Random.Range(-flockingData.FlockLimits.x, flockingData.FlockLimits.x),
                                                        Random.Range(-flockingData.FlockLimits.y, flockingData.FlockLimits.y),
                                                        Random.Range(-flockingData.FlockLimits.z, flockingData.FlockLimits.z));
            }

            for (int i = 0; i < flockingData.allUnits.Length; i++)
            {
                UnitData unitData = flockingData.allUnits[i];

                Vector3 direction = Vector3.zero;
                Bounds bounds = new Bounds(flockingData.FlockGoal, flockingData.FlockLimits * 2);
                Vector3? nullableHitDirection = CheckForCollision(unitData.transform);

                // Rotate unit back towards the flock if they move outside of flock limit
                if (CheckFlockBounds(unitData.transform.position, bounds) && checkForBounds)
                {
                    direction = flockingData.FlockGoal - unitData.transform.position;
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }
                // Move the fish back into the water if they move their max mesh bounds outside the top part
                else if (CheckFlockBounds(new Vector3(unitData.transform.position.x, unitData.meshRenderer.bounds.max.y, unitData.meshRenderer.bounds.max.z), flockingData.WaterBounds))
                {
                    //Debug.LogError($"Fish {unitData.transform.name} has max mesh bounds outside top part of water!");
                    //direction = new Vector3(unitData.transform.position.x, flockingData.WaterBounds.center.y, unitData.transform.position.z) - unitData.transform.position;
                    //direction = Vector3.Reflect(unitData.transform.forward, new Vector3(unitData.transform.position.x, flockingData.WaterBounds.max.y, unitData.transform.position.z));
                    //direction = new Vector3(unitData.transform.position.x, flockingData.WaterBounds.center.y, unitData.transform.position.z) - unitData.transform.position + unitData.transform.forward;

                    direction = Vector3.Slerp(unitData.transform.forward, new Vector3(unitData.transform.forward.x, flockingData.WaterBounds.center.y - unitData.transform.position.y, unitData.transform.forward.z), 0.3f);
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }
                // Move the fish back into the water if they move their min mesh bounds outside the bottom part
                else if (CheckFlockBounds(new Vector3(unitData.transform.position.x, unitData.meshRenderer.bounds.min.y, unitData.meshRenderer.bounds.max.z), flockingData.WaterBounds))
                {
                    //Debug.LogError($"Fish {unitData.transform.name} has min mesh bounds outside bottom part of water!");
                    //direction = new Vector3(unitData.transform.position.x, flockingData.WaterBounds.center.y, unitData.transform.position.z) - unitData.transform.position;
                    //direction = Vector3.Reflect(unitData.transform.forward, new Vector3(unitData.transform.position.x, flockingData.WaterBounds.min.y, unitData.transform.position.z));
                    //direction = new Vector3(unitData.transform.position.x, flockingData.WaterBounds.center.y, unitData.transform.position.z) - unitData.transform.position + unitData.transform.forward;

                    direction = Vector3.Slerp(unitData.transform.forward, new Vector3(unitData.transform.forward.x, flockingData.WaterBounds.center.y - unitData.transform.position.y, unitData.transform.forward.z), 0.3f);
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }
                // Rotate unit away to avoid obstacles
                else if (nullableHitDirection != null && checkForCollision)
                {
                    direction = (Vector3)nullableHitDirection;
                    Debug.DrawRay(unitData.transform.position, direction);
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }
                // Apply regular flocking behavior based on random frequency
                else
                {
                    if (Random.Range(0, 100) < 1)
                    {
                        unitData.speed = Random.Range(flockingData.unitMinSpeed, flockingData.unitMaxSpeed);
                    }

                    if (Random.Range(0, 100) < flockingData.flockingFrequency)
                    {
                        direction = FlockDirection(unitData, flockingData);
                    }
                }

                if (direction != Vector3.zero)
                {
                    unitData.transform.rotation = Quaternion.Slerp(unitData.transform.rotation, Quaternion.LookRotation(direction), flockingData.unitRotationSpeed * Time.deltaTime);
                }

                unitData.transform.Translate(0, 0, Time.deltaTime * unitData.speed);
            }
        }

        public static Vector3 FlockDirection(UnitData unitData, FlockingData flockingData)
        {
            Vector3 headingVector = Vector3.zero;
            Vector3 avoidanceVector = Vector3.zero;
            Vector3 direction = Vector3.zero;

            float groupSpeed = 0.01f;
            float minDistanceToGroup;
            int groupSize = 0;

            foreach (var unit in flockingData.allUnits)
            {
                if (unit.transform != unitData.transform)
                {
                    // Check distance to each fish in flock
                    minDistanceToGroup = Vector3.Distance(unit.transform.position, unitData.transform.position);
                    if (minDistanceToGroup <= flockingData.maximumGroupDistance)
                    {
                        //Add that fish to group & add their position to group avg (flock center avg)
                        headingVector += unit.transform.position;
                        groupSize++;

                        // Check if distance is bigger than minimum distance value
                        if (minDistanceToGroup < flockingData.minimumUnitProximity)
                        {
                            // Include that unit's position to the avoidance vector (don't hit neighbours)
                            avoidanceVector = avoidanceVector + (unitData.transform.position - unit.transform.position);
                        }

                        // TEMPORARY - Grab the speed of each unit and add it to the global speed value (determine global flock speed)
                        groupSpeed = groupSpeed + unitData.speed;
                    }
                }
            }

            if (groupSize > 0)
            {
                // Find the avg vector of the flock's center based on group size
                headingVector = headingVector / groupSize;

                // If a goal position is set, calculate the flock's heading based on goal location
                if (flockingData.FlockGoal != null)
                {
                    headingVector = headingVector + (flockingData.FlockGoal - unitData.transform.position);
                }

                // Set the individual's speed to the avg group speed
                unitData.speed = groupSpeed / groupSize;

                // Determine the direction the fish wants to travel to (based on center of flock & avoidance of neighbours)
                direction = (headingVector + avoidanceVector) - unitData.transform.position;
            }

            return direction;
        }

        public static Vector3? CheckForCollision(Transform currentUnit)
        {
            Vector3 forward = (currentUnit.forward).normalized;
            Vector3 up = (currentUnit.up + currentUnit.forward).normalized;
            Vector3 down = (-currentUnit.up + currentUnit.forward).normalized;

            var rc = Physics.Raycast(currentUnit.position, forward, out RaycastHit hitInfo, 1f);
            Debug.DrawRay(currentUnit.position, forward, Color.red);

            var rcUp = Physics.Raycast(currentUnit.position, up, out RaycastHit hitInfoUp, 1f);
            Debug.DrawRay(currentUnit.position, up, Color.red);

            var rcDown = Physics.Raycast(currentUnit.position, down, out RaycastHit hitInfoDown, 1f);
            Debug.DrawRay(currentUnit.position, down, Color.red);

            Vector3? direction = null;
            if (rc) 
            { 
                //Debug.Log($"{currentUnit.name} hit {hitInfo.collider.gameObject.name} || Axis: Forward.");
                direction = Vector3.Reflect(forward, hitInfo.normal);
            }
            else if (rcUp) 
            { 
                //Debug.Log($"{currentUnit.name} hit {hitInfoUp.collider.gameObject.name} || Axis: Up.");
                direction = Vector3.Reflect(up, hitInfoUp.normal);
            }
            else if (rcDown) 
            { 
                //Debug.Log($"{currentUnit.name} hit {hitInfoDown.collider.gameObject.name} || Axis: Down.");
                direction = Vector3.Reflect(down, hitInfoDown.normal);
            }

            return direction;
        }

        public static bool CheckFlockBounds(Vector3 position, Bounds bounds)
        {
            if (!bounds.Contains(position))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }

    #region FLOCKING_CLASS
    [System.Serializable]
    public class FlockingData
    {
        [Header("Unit Details")]
        public List<GameObject> unitPrefabs = new List<GameObject>();
        public int minUnits = 10;
        public int maxUnits = 20;
        [Range(0.0f, 5.0f)] public float unitMinSpeed = 1f;
        [Range(0.0f, 5.0f)] public float unitMaxSpeed = 2f;
        [Range(0.0f, 5.0f)] public float unitRotationSpeed = 1f;

        [Header("Flock Location")]
        [SerializeField] private Transform flockCenter;
        public Transform FlockCenter => flockCenter;
        [SerializeField] private Vector3 flockLimits = new Vector3(5f, 5f, 5f);
        public Vector3 FlockLimits => flockLimits * flockLimitSizeMultiplier;
        [Range(1.0f, 10.0f)] public float flockLimitSizeMultiplier = 1f;

        [Header("Flock Movement")]
        [Range(0.0f, 100.0f)] public float positionGoalChangeFrequency = 1f;
        [Range(0.0f, 100.0f)] public float flockingFrequency = 20f;
        [Range(0.0f, 5.0f)] public float minimumUnitProximity = 1f;
        [Range(1.0f, 10.0f)] public float maximumGroupDistance = 1f;

        [Header("Water Bounds")]
        public Water water;
        public Bounds WaterBounds => water.Collider.bounds;
        public UnitData[] allUnits;
        public Vector3 FlockGoal { get; set; }        

        public void AdjustGoalWithinFlockRange(float x, float y, float z)
        {
            FlockGoal = flockCenter.position + new Vector3(x, y, z);
            //Debug.Log($"The flock's goal position has been set to {FlockGoal}.");
        }
    }

    public struct UnitData
    {
        public Transform transform;
        public Renderer meshRenderer;
        public float speed;
    }
    #endregion
}