using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    private bool isActive => gameObject.activeSelf;

    public void Toggle()
    {
        if (isActive)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
        
    }
}
