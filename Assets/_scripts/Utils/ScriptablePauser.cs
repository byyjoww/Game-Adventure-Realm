using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Scriptable Pauser")]
public class ScriptablePauser : ScriptableObject
{
    public void SetTimeScale(float i)
    {
        Time.timeScale = i;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Unpause()
    {
        Time.timeScale = 1;
    }
}
