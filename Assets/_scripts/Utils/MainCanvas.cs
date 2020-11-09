using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnityPhase { AWAKE, ON_ENABLE, START, UPDATE, FIXED_UPDATE, LATE_UPDATE, ON_DISABLE, ON_DESTROY };

public class MainCanvas : MonoBehaviour
{
    public UnityPhase phaseToDisable;
    public List<GameObject> objectsToDisable = new List<GameObject>();

    private void Awake()
    {
        if (phaseToDisable == UnityPhase.AWAKE) { DisableObjects(); }
    }

    void Start()
    {
        if (phaseToDisable == UnityPhase.START) { DisableObjects(); }
    }

    private void OnEnable()
    {
        if (phaseToDisable == UnityPhase.ON_ENABLE) { DisableObjects(); }
    }

    private void OnDisable()
    {
        if (phaseToDisable == UnityPhase.ON_DISABLE) { DisableObjects(); }
    }

    private void OnDestroy()
    {
        if (phaseToDisable == UnityPhase.ON_DESTROY) { DisableObjects(); }
    }

    private void DisableObjects()
    {
        for (int i = 0; i < objectsToDisable.Count; i++)
        {
            objectsToDisable[i].SetActive(false);
        }
    }
}
