using System;
using System.Linq;
using UnityEngine;

public class PersistentAttribute : Attribute { }
public class CreateFromPrefabAttribute : Attribute
{
    public CreateFromPrefabAttribute(string prefab)
    {
        this.prefab = prefab;
    }
    public readonly string prefab;
}

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _instance;
    private static readonly object Lock = new object();

    public static T Instance
    {
        get
        {
            if (_quitting) { return null; }
            lock (Lock)
            {
                if (_instance == null)
                {
                    // Debug.LogError($"INITIALIZING SINGLETON OF TYPE {typeof(T).ToString()}.");
                    T[] handlers = FindObjectsOfType<T>();
                    if (handlers.Length == 0)
                    {
                        if (typeof(T).GetCustomAttributes(typeof(CreateFromPrefabAttribute), true).Length > 0)
                        {
                            CreateFromPrefabAttribute attr = typeof(T).GetCustomAttributes(typeof(CreateFromPrefabAttribute), true).Single() as CreateFromPrefabAttribute;
                            Debug.LogWarning("Loading Singleton:" + attr.prefab);
                            _instance = Instantiate(Resources.Load<T>(attr.prefab));
                        }
                        else
                            _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                    else if (handlers.Length == 1)
                    {
                        _instance = handlers[0];
                    }                        
                    else
                    {
                        throw new System.Exception(typeof(T) + ": There should never be more than one Singleton at once!");
                    }
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (typeof(T).GetCustomAttributes(typeof(PersistentAttribute), true).Length > 0)
        {
            if(_instance == null)
            {
                var i = Instance;
                DontDestroyOnLoad(_instance);
            }            
        }            
    }

    protected virtual void OnValidate()
    {
        //if (_instance == null)
        //    _instance = this as T;

        //else if (_instance != this)
        //    throw new System.Exception("Singleton already initialized!");
    }

    protected virtual void OnDestroy()
    {
        if(_instance == this) { _instance = null; }            
    }

    private static bool _quitting = false;
    private void OnApplicationQuit()
    {
        _quitting = true;
    }
}
