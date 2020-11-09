using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

namespace Elysium.Shop
{
    public class ShopItemLayoutElement : MonoBehaviour
    {
        [SerializeField] TMP_Text nameTextComponent;
        [SerializeField] TMP_Text priceTextComponent;
        [SerializeField] TMP_Text rarityTextComponent;
        [SerializeField] TMP_Text typeTextComponent;
        [SerializeField] TMP_Text quantityTextComponent;
        [SerializeField] Image imageComponent;
        [SerializeField] Button buttonComponent;
        [SerializeField] GameObject itemDetailsPrefab;

        public void Setup(Item item, int price, Action action, string buttonText)
        {
            nameTextComponent.text = item.ItemName;
            priceTextComponent.text = price.ToString();

            rarityTextComponent.color = item.ItemRarity.RarityTextColor;
            rarityTextComponent.text = item.ItemRarity.itemRarityLevel.ToString();

            typeTextComponent.color = item.ItemType.TypeTextColor;
            typeTextComponent.text = item.ItemType.itemType.ToString();

            imageComponent.sprite = item.ItemSprite;
            buttonComponent.onClick.AddListener(delegate { ButtonAction(item, price, action, buttonText); });
        }

        public void Setup(Item item, int price, Action action, string buttonText, int stackQuantity)
        {
            nameTextComponent.text = item.ItemName;
            priceTextComponent.text = price.ToString();

            rarityTextComponent.color = item.ItemRarity.RarityTextColor;
            rarityTextComponent.text = item.ItemRarity.itemRarityLevel.ToString();

            typeTextComponent.color = item.ItemType.TypeTextColor;
            typeTextComponent.text = item.ItemType.itemType.ToString();

            imageComponent.sprite = item.ItemSprite;
            buttonComponent.onClick.AddListener(delegate { ButtonAction(item, price, action, buttonText); });

            quantityTextComponent.gameObject.SetActive(true);
            quantityTextComponent.text = $"Owned: {stackQuantity}x";
        }

        public static GameObject Create(GameObject prefab, Transform transform, Item item, int price, Action action, string buttonText = "Buy")
        {
            GameObject obj = Instantiate(prefab, transform);

            ShopItemLayoutElement element = obj.GetComponent<ShopItemLayoutElement>();
            element.Setup(item, price, action, buttonText);

            return obj;
        }

        public static GameObject Create(GameObject prefab, Transform transform, Item item, int price, Action action, int stackQuantity, string buttonText = "Buy")
        {
            GameObject obj = Instantiate(prefab, transform);

            ShopItemLayoutElement element = obj.GetComponent<ShopItemLayoutElement>();
            element.Setup(item, price, action, buttonText, stackQuantity);

            return obj;
        }

        public void ButtonAction(Item item, int price, Action action, string buttonText)
        {
            ShopItemPurchaseDetailsPanel.Create(itemDetailsPrefab, item.ItemName, item.ItemDescription, item.ItemSprite, price, action, buttonText);
        }
    }
}