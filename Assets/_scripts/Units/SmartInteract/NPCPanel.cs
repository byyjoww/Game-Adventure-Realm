using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPanel : MonoBehaviour
{
    public event Action OnObjectActivated;
    public event Action OnObjectDeactivated;

    private void OnEnable() => OnObjectActivated?.Invoke();
    private void OnDisable() => OnObjectDeactivated?.Invoke();

    public void SetActive(bool _state) => gameObject.SetActive(_state);
}
