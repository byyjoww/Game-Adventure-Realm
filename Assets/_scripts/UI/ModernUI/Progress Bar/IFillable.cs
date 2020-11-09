using System;
using UnityEngine;

public interface IFillable
{
    float CurrentFill { get; }
    float MaxFill { get; }
    event Action OnFillValueChanged;
}
