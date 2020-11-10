using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmartObject : MonoBehaviour
{
    public bool Focusable => focusable;
    protected bool focusable = false;
    public bool Interactable => interactable;
    protected bool interactable = false;

    public event Action OnInteraction;
    public event Action OnFocusSet;
    public event Action OnFocusRemoved;

    public virtual void TryInteract(SmartController smartController)
    {
        if (!interactable) { return; }
        OnInteraction?.Invoke();
        Interact(smartController);
    }

    public abstract void Interact(SmartController smartController);

    public virtual void SetFocus()
    {
        if (!focusable) { return; }
        OnFocusSet?.Invoke();
    }

    public virtual void RemoveFocus() => OnFocusRemoved?.Invoke();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}
