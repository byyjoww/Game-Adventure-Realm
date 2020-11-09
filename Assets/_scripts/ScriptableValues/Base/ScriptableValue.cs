using System;
using UnityEngine;

public abstract class ScriptableValue : ScriptableObject
{
    public abstract string ValueAsString { get; }
    
#pragma warning disable 0067
    public event Action OnValueChanged;
#pragma warning restore 0067

    protected void InvokeOnValueChanged()
    {
        OnValueChanged?.Invoke();
    }
}
