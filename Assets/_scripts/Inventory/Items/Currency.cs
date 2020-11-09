using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new Currency", menuName = "Scriptable Objects/Item/Currency")]
public class Currency : ScriptableObject
{
    public string currencyName;
    [SerializeField] private IntValue currencyAmount;
    public IntValue CurrencyAmount => currencyAmount;

    public bool HaveEnoughCurrency(int amount)
    {
        return amount <= currencyAmount.Value;
    }

    public void GetCurrency(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        currencyAmount.Value += amount;
        Debug.Log($"Player gained {amount} {currencyName}.");
    }

    public bool SpendCurrency(int amount)
    {
        if (!HaveEnoughCurrency(amount))
        {
            Debug.Log($"Player has insufficient {currencyName}.");
            return false;
        }

        if (amount < 0)
        {
            throw new ArgumentException("Invalid value for <amount>. Value must be a positive integer.");
        }

        currencyAmount.Value -= amount;
        Debug.Log($"Player spent {amount} {currencyName}.");

        return true;
    }
}