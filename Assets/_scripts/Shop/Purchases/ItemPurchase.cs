using Elysium.Attributes;
using UnityEngine;

namespace Elysium.Shop
{
    [CreateAssetMenu(fileName = "new Item Purchase", menuName = "Scriptable Objects/Shop/Item Purchase")]
    public class ItemPurchase : Purchase
    {
        [RequireInterface(typeof(IInventoryElement))]
        public Item item;
        public Inventory itemInventory;
        public int quantity = 1;

        public GameObject prefab;

        protected override void PurchaseSuccessful()
        {
            itemInventory.GainItem(item as IInventoryElement, quantity);
        }

        public override GameObject CreateUIElement(Transform tr)
        {
            return ShopItemLayoutElement.Create(prefab, tr, item, purchasePrice, Buy);
        }
    }
}