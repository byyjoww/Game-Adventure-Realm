using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventoryElement
{
    string ItemName { get; }
    string ItemDescription { get; }
    Sprite ItemSprite { get; }
    bool IsStackable { get; }
    int ItemValue { get; }
}
