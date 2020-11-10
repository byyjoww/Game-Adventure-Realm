using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Shop
{
    [CreateAssetMenu(fileName = "new Crate Purchase", menuName = "Scriptable Objects/Shop/Crate Purchase")]
    public class CratePurchase : ManualPurchase
    {
        [SerializeField] private Crate crate;
        public Inventory itemInventory;

        protected override void PurchaseSuccessful()
        {
            foreach (var resultData in crate.CrateContents)
            {
                itemInventory.GainItem(resultData.item as IInventoryElement, resultData.quantity);
                // PopupSystem.CreatePopup("New Item", $"You have received x{resultData.quantity} {resultData.item.ItemName}.");
            }
        }
    }
}