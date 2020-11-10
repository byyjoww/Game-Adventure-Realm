using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    [SerializeField] UnityPhase phase;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (phase != UnityPhase.UPDATE) { return; }
        Align();
    }

    private void FixedUpdate()
    {
        if (phase != UnityPhase.FIXED_UPDATE) { return; }
        Align();
    }

    private void LateUpdate()
    {
        if (phase != UnityPhase.LATE_UPDATE) { return; }
        Align();
    }

    private void Align() => transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
}
