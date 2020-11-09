using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.Characters.ThirdPerson;

public class WaterDetection : MonoBehaviour
{
    Camera cam;    
    [SerializeField] FirstPersonController controller;

    private void Start()
    {
        cam = Camera.main;
        Physics.IgnoreLayerCollision(gameObject.layer, 4);
    }

    private void Update()
    {
        if (Singleton_Water.Instance.SwimmingCheck(cam.transform.position)) { controller.Swimming = true; }
        else { controller.Swimming = false; }
    }
}
