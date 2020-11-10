using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Collider collider;
    public Collider Collider => collider;

    private void OnValidate()
    {
        if (collider == null) collider = GetComponent<Collider>();
    }
}
