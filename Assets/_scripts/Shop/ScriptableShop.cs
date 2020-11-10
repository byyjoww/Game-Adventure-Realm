using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Shop
{
    [CreateAssetMenu(fileName = "Shop", menuName = "Scriptable Objects/Shop/Shop")]
    public class ScriptableShop : ScriptableObject
    {
        public List<UI_ShopBuy.ShopBuySection> shopSections = new List<UI_ShopBuy.ShopBuySection>();
    }
}
