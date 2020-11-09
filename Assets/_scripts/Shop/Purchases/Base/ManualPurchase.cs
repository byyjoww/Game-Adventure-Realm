using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Shop
{
    public abstract class ManualPurchase : Purchase
    {
        public string purchaseName;
        public string purchaseDescription;
        public Sprite purchaseSprite;
        public GameObject prefab;

        public override GameObject CreateUIElement(Transform tr)
        {
            return ShopManualLayoutElement.Create(prefab, tr, purchaseName, purchaseDescription, purchaseSprite, purchasePrice, Buy);
        }
    }
}