using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableEventListener : MonoBehaviour
{
    [SerializeField] protected ScriptableEvent Event;

    [SerializeField] protected UnityEvent Response;

    void OnEnable()
    {
        Event.OnRaise += OnEventRaised;
    }

    void OnDisable()
    {
        Event.OnRaise -= OnEventRaised;
    }

    public void OnEventRaised()
    {
        Response.Invoke();
    }
}
