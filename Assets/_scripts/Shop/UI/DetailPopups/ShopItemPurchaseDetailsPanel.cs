using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Elysium.Shop
{
    public class ShopItemPurchaseDetailsPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text nameTextComponent;
        [SerializeField] TMP_Text descriptionTextComponent;
        [SerializeField] TMP_Text buttonTextComponent;
        [SerializeField] Image imageComponent;
        [SerializeField] Button buttonComponent;

        public void Setup(string name, string description, Sprite sprite, int price, Action action, string buttonText)
        {
            nameTextComponent.text = name;
            descriptionTextComponent.text = description;
            buttonTextComponent.text = buttonText;
            imageComponent.sprite = sprite;
            buttonComponent.onClick.AddListener(delegate { ButtonAction(action); });
        }

        public static GameObject Create(GameObject prefab, string name, string description, Sprite sprite, int price, Action action, string buttonText)
        {
            GameObject obj = Instantiate(prefab);

            ShopItemPurchaseDetailsPanel element = obj.GetComponent<ShopItemPurchaseDetailsPanel>();
            element.Setup(name, description, sprite, price, action, buttonText);

            return obj;
        }

        public void ButtonAction(Action action)
        {
            action?.Invoke();
        }
    }
}