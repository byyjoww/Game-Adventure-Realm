using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WindowOpener : MonoBehaviour
{
    public GameObject inventory;
    public Dictionary<KeyCode, GameObject> HotkeyDictionary;

    private void Start()
    {
        HotkeyDictionary = new Dictionary<KeyCode, GameObject>()
        {
            { KeyCode.I, inventory },            
        };
    }

    void Update()
    {
        foreach (var key in HotkeyDictionary.Keys)
        {
            if (Input.GetKeyDown(key))
            {
                var go = HotkeyDictionary[key];
                go.SetActive(!go.activeSelf);
            }
        }
    }
}
