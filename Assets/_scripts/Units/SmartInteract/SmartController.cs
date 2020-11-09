using Elysium.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SmartController : MonoBehaviour
{
    public Inventory Inventory => inventory;
    [SerializeField] private Inventory inventory;
    [SerializeField, Range(0, 100)] private float interactRadius;

    private List<SmartObject> smartObjectsInRange;
    private SmartObject InteractableObject { get; set; }

    private void Start() => smartObjectsInRange = new List<SmartObject>();

    private void Update()
    {
        CheckForSmartObjects();
        CheckForClosestObject();
        CheckForInput();
    }

    private void CheckForSmartObjects()
    {
        smartObjectsInRange.Clear();

        var colliders = Physics.OverlapSphere(transform.position, interactRadius);
        if (colliders == null || colliders.Length < 1) { return; }

        for (int i = 0; i < colliders.Length; i++)
        {
            var obj = colliders[i].GetComponent<SmartObject>();
            if (obj != null) { smartObjectsInRange.Add(obj); }
        }
    }

    private void CheckForClosestObject()
    {
        if (smartObjectsInRange.Count < 1) { SetSmartObject(null); return; }
        if (smartObjectsInRange.Count == 1) { SetSmartObject(smartObjectsInRange[0]); return; }

        float distance = Mathf.Infinity;
        SmartObject closestObject = null;

        for (int i = 0; i < smartObjectsInRange.Count; i++)
        {
            var thisObjectsDistance = Vector3.Distance(transform.position, smartObjectsInRange[i].transform.position);
            if (thisObjectsDistance < distance)
            {
                distance = thisObjectsDistance;
                closestObject = smartObjectsInRange[i];
            }
        }

        SetSmartObject(closestObject);
    }

    private void SetSmartObject(SmartObject newObject)
    {
        if (InteractableObject != null) { InteractableObject.RemoveFocus(); }
        InteractableObject = newObject;
        if (InteractableObject != null && InteractableObject.Focusable) { InteractableObject.SetFocus(); }
    }

    private void CheckForInput()
    {
        if (InteractableObject == null) { return; }
        if (!InteractableObject.Interactable) { return; }

        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractableObject.TryInteract(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
