using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePause : MonoBehaviour
{
    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void PauseWithDelay(float delay)
    {
        Tools.Invoke(this, Pause, delay);        
    }

    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
