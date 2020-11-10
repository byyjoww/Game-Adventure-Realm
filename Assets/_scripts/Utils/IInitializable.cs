using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializable
{
    void Init();
    bool Initialized { get; }
}
