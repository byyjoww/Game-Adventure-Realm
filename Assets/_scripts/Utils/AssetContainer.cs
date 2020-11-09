using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class AssetContainer<T> : ScriptableObject where T : ScriptableObject
{
    [HideInInspector][SerializeField]
    public List<T> Nodes;
    
    public virtual string NodeTooltip(int i) { return ""; } 

    public virtual void OnEnable()
    {
        if (Nodes == null) Nodes = new List<T>();
    }

    public virtual void OnValidate()
    {
        //Debug.Log("Asset container validate!");
    }
}

