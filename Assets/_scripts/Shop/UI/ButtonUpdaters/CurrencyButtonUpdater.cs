using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Elysium.Shop
{
    public abstract class CurrencyButtonUpdater : MonoBehaviour
    {
        [SerializeField] public int currencyPrice;
        [SerializeField] protected TMP_Text buttonLabel;
        [SerializeField] protected Currency currency;

        [SerializeField] protected UnityEvent OnPurchaseSucceeded;
        [SerializeField] protected UnityEvent OnPurchaseFailed;

        private void Awake()
        {
            buttonLabel.text = currencyPrice.ToString();
        }

        public abstract void Buy();

        private void OnValidate()
        {
            if (buttonLabel == null) buttonLabel = GetComponentInChildren<TMP_Text>();
        }
    }
}