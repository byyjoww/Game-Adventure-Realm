using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;

public class WaterDetection : MonoBehaviour
{
    private Camera cam;    
    private FirstPersonController controller;
    [SerializeField] private Collider groundCollider;

    private void Start()
    {
        cam = Camera.main;
        controller = GetComponent<FirstPersonController>();
        Physics.IgnoreLayerCollision(gameObject.layer, 4);
    }

    private void Update()
    {
        if (Singleton_Water.Instance.SwimmingCheck(groundCollider.transform.position)) { controller.OnWater = true; }
        else { controller.OnWater = false; }

        if (Singleton_Water.Instance.SwimmingCheck(cam.transform.position)) { controller.Swimming = true; }
        else { controller.Swimming = false; }
    }
}
