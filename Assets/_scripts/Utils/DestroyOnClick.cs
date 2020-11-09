using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnClick : MonoBehaviour
{
    public void DestroyThis(GameObject obj)
    {
        Destroy(obj);
    }
}
