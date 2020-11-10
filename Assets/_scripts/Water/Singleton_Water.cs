using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Singleton_Water : Singleton<Singleton_Water>
{
    [SerializeField] Vector3 detectionOffset = new Vector3(0f, - 1f, 0f);
    public List<Water> waterBodies = new List<Water>();

    public bool SwimmingCheck(Vector3 pos)
    {
        foreach (var waterbody in waterBodies)
        {
            if (waterbody.Collider.bounds.Contains(pos + detectionOffset))
            {
                //Debug.Log($"Point: {pos} is located underwater.");
                return true;
            }
        }

        //Debug.Log($"Point: {pos} is located on land.");
        return false;
    }

    public bool UnderwaterCheck(Vector3 pos)
    {
        foreach (var waterbody in waterBodies)
        {
            if (waterbody.Collider.bounds.Contains(pos))
            {
                //Debug.Log($"Point: {pos} is located underwater.");
                return true;
            }            
        }

        //Debug.Log($"Point: {pos} is located on land.");
        return false;
    }

    private void OnValidate()
    {
        waterBodies = FindObjectsOfType<Water>().ToList();
    }
}
