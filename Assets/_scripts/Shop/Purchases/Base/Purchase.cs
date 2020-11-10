using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Elysium.Shop
{
    public abstract class Purchase : ScriptableObject
    {
        [SerializeField] protected int purchasePrice;
        [SerializeField] protected Currency currency;
        public ScriptableEvent OnPurchaseFailed;
        public ScriptableEvent OnPurchaseSucceeded;

        protected void Buy()
        {
            if (PurchaseCondition())
            {
                if (currency.SpendCurrency(purchasePrice))
                {
                    // PopupSystem.CreatePopup("Success", "Your purchase was successful!");
                    OnPurchaseSucceeded.Raise();
                    PurchaseSuccessful();
                }
                else
                {
                    // PopupSystem.CreatePopup("Error", $"Your purchase failed. Please make sure you have enough {currency.currencyName}.");
                    OnPurchaseFailed.Raise();
                }
            }
            else
            {
                // PopupSystem.CreatePopup("Error", ConditionMessage);
                OnPurchaseFailed.Raise();
            }
        }

        protected virtual bool PurchaseCondition()
        {
            return true;
        }
        protected virtual string ConditionMessage { get { return "The purchase failed as you didn't meet the required conditions"; } }

        protected abstract void PurchaseSuccessful();
        public abstract GameObject CreateUIElement(Transform tr);
    }
}