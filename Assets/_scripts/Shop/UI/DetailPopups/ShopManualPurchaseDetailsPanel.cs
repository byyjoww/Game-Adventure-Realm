using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Elysium.Shop
{
    public class ShopManualPurchaseDetailsPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text nameTextComponent;
        [SerializeField] TMP_Text descriptionTextComponent;
        [SerializeField] Image imageComponent;
        [SerializeField] Button buttonComponent;

        public void Setup(string name, string description, Sprite sprite, int price, Action action)
        {
            nameTextComponent.text = name;
            descriptionTextComponent.text = description;
            imageComponent.sprite = sprite;
            buttonComponent.onClick.AddListener(delegate { ButtonAction(action); });
        }

        public static GameObject Create(GameObject prefab, string name, string description, Sprite sprite, int price, Action action)
        {
            GameObject obj = Instantiate(prefab);

            ShopManualPurchaseDetailsPanel element = obj.GetComponent<ShopManualPurchaseDetailsPanel>();
            element.Setup(name, description, sprite, price, action);

            return obj;
        }

        public void ButtonAction(Action action)
        {
            action?.Invoke();
        }
    }
}