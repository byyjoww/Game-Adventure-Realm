using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elysium.AI;

public class AIFlockManager : MonoBehaviour
{
    public Color gizmosColor = Color.red;
    public FlockingData flockingData = new FlockingData();
    private Transform gizmos;

    private void Start()
    {
        AICrowdSimulation.CreateFlock(flockingData, transform);
    }

    private void Update()
    {
        AICrowdSimulation.Flock(flockingData);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = gizmosColor;        
        //Gizmos.DrawCube(transform.position, flockingData.FlockLimits * flockingData.flockLimitSizeMultiplier);

        DrawCube();
    }

    private void DrawCube()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            if (gizmos == null)
            {
                // create one
                var prefab = Resources.Load("GizmosCube");
                if (!prefab)
                {
                    Debug.LogError("Could not load Gizmos");
                }
                else
                {
                    gizmos = (Instantiate(prefab, transform) as GameObject).transform;
                    gizmos.gameObject.hideFlags = HideFlags.HideAndDontSave;
                }
            }
            // position it
            if (gizmos.position != transform.position) { gizmos.position = transform.position; }
            if (gizmos.rotation != transform.rotation) { gizmos.rotation = transform.rotation; }

            gizmos.localScale = GizmosSize();
        }
    }

    private Vector3 GizmosSize()
    {
        return new Vector3(flockingData.FlockLimits.x * flockingData.flockLimitSizeMultiplier,
                           flockingData.FlockLimits.y * flockingData.flockLimitSizeMultiplier,
                           flockingData.FlockLimits.z * flockingData.flockLimitSizeMultiplier);
    }
}
