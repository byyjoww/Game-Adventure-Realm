using System;
using UnityEngine;

public class GenericScriptableEvent<T> : ScriptableObject
{
    
#pragma warning disable 0067
    public event Action<T> OnRaise;
#pragma warning restore 0067
    
    public void Raise(T data)
    {
        OnRaise?.Invoke(data);
    }

    public event Action OnRequestList;

    public void RequestRaise()
    {
        OnRequestList?.Invoke();
    }
}

public class GenericScriptableEvent<T, R> : ScriptableObject
{

#pragma warning disable 0067
    public event Action<T, R> OnRaise;
#pragma warning restore 0067

    public void Raise(T data, R data2)
    {
        OnRaise?.Invoke(data, data2);
    }

    public event Action OnRequestList;

    public void RequestRaise()
    {
        OnRequestList?.Invoke();
    }
}

public class GenericScriptableEvent<T, R, V> : ScriptableObject
{

#pragma warning disable 0067
    public event Action<T, R, V> OnRaise;
#pragma warning restore 0067

    public void Raise(T data, R data2, V data3)
    {
        OnRaise?.Invoke(data, data2, data3);
    }

    public event Action OnRequestList;

    public void RequestRaise()
    {
        OnRequestList?.Invoke();
    }
}

public class GenericScriptableEvent<T, R, V, Z> : ScriptableObject
{

#pragma warning disable 0067
    public event Action<T, R, V, Z> OnRaise;
#pragma warning restore 0067

    public void Raise(T data, R data2, V data3, Z data4)
    {
        OnRaise?.Invoke(data, data2, data3, data4);
    }

    public event Action OnRequestList;

    public void RequestRaise()
    {
        OnRequestList?.Invoke();
    }
}