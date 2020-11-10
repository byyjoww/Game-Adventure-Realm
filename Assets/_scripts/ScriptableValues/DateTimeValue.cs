using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DateTimeValue", menuName = "Scriptable Objects/Scriptable Value/Complex/DateTime", order = 1)]
public class DateTimeValue : GenericValue<DateTime>
{
    void Awake()
    {
        Value = DateTime.UtcNow;
    }
}
