using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActiveStateSync : MonoBehaviour
{
    public List<GameObject> objectsToSyncOnEnable = new List<GameObject>();
    public List<GameObject> objectsToSyncOnDisable = new List<GameObject>();

    private void OnEnable()
    {
        for (int i = 0; i < objectsToSyncOnEnable.Count; i++)
        {
            SyncActiveState(objectsToSyncOnEnable, i);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < objectsToSyncOnDisable.Count; i++)
        {
            SyncActiveState(objectsToSyncOnDisable, i);
        }
    }

    private void SyncActiveState(List<GameObject> list, int i)
    {
        list[i].SetActive(gameObject.activeSelf);
    }
}
