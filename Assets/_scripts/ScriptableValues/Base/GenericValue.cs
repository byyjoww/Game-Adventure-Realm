using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenericValue<T> : ScriptableValue
{
    [SerializeField] private T value;
    [SerializeField] protected T defaultValue;
    public T Value
    {
        get
        {
            return value;
        }

        set
        {
            //Debug.Log($"{name} Changing Value to {value}.");
            this.value = value;
            InvokeOnValueChanged();
        }
    }    

    public override string ValueAsString => Value.ToString();
}
