using Elysium.Attributes;
using System;
using UnityEngine;

namespace Elysium.Stats
{
    [System.Serializable]
    public class CharacterStat
    {
        public enum StatType { STR = 0, AGI = 1, INT = 2, VIT = 3, DEX = 4, LUK = 5 }

        public int Value => value;
        [SerializeField] private int value = 1;
        public StatType StatName => statName;
        [SerializeField, ReadOnly] private StatType statName;

        public int BuffCount { get; private set; }
        public int DebuffCount { get; private set; }

        private const int MIN_VALUE = 1;
        private const int MAX_VALUE = 1000;

        public event Action<StatType, int> OnBuffStatusChange;
        public event Action<StatType, int> OnStatIncrease;
        public event Action<StatType, int> OnStatChanged;

        public CharacterStat(StatType statName, int value)
        {
            this.value = value;
            this.statName = statName;
            BuffCount = 0;
            DebuffCount = 0;
        }

        public void Increase(int increaseAmount)
        {
            this.value += increaseAmount;
            OnStatIncrease?.Invoke(StatName, value);
            OnStatChanged?.Invoke(StatName, value);
        }

        public void BuffFlat(int increaseAmount, float timeToIncreaseFor)
        {
            int newValue = value + increaseAmount;
            newValue = Mathf.Clamp(newValue, MIN_VALUE, MAX_VALUE);
            int actualIncreaseAmount = newValue - value;

            value += actualIncreaseAmount;
            BuffCount++;
            OnBuffStatusChange?.Invoke(StatName, value);
            OnStatChanged?.Invoke(StatName, value);
            if (value > MAX_VALUE) { throw new System.Exception("value is higher than max value"); }

            Tools.DelayedExecution(() => { value -= actualIncreaseAmount; BuffCount--; OnBuffStatusChange?.Invoke(StatName, value); OnStatChanged?.Invoke(StatName, value); }, timeToIncreaseFor);
        }

        public void DebuffFlat(int decreaseAmount, float timeToDecreaseFor)
        {
            int newValue = value - decreaseAmount;
            newValue = Mathf.Clamp(newValue, MIN_VALUE, MAX_VALUE);
            int actualDecreaseAmount = newValue + value;

            value -= actualDecreaseAmount;
            DebuffCount++;
            OnBuffStatusChange?.Invoke(StatName, value);
            OnStatChanged?.Invoke(StatName, value);
            if (value < MIN_VALUE) { throw new System.Exception("value is lower than min value"); }

            Tools.DelayedExecution(() => { value += actualDecreaseAmount; DebuffCount--; OnBuffStatusChange?.Invoke(StatName, value); OnStatChanged?.Invoke(StatName, value); }, timeToDecreaseFor);
        }

        public void BuffPercent(float increasePercent, float timeToIncreaseFor)
        {
            int flatIncrease = Mathf.RoundToInt(value * (increasePercent / 100));
            BuffFlat(flatIncrease, timeToIncreaseFor);
        }

        public void DebuffPercent(float decreasePercent, float timeToDecreaseFor)
        {
            int flatDecrease = Mathf.RoundToInt(value * (decreasePercent / 100));
            DebuffFlat(flatDecrease, timeToDecreaseFor);
        }
    }

    [System.Serializable]
    public class CharacterStatWrapper
    {
        public CharacterStatWrapper(CharacterStat.StatType stat, int amount)
        {
            this.StatName = stat;
            Value = amount;
        }

        [ReadOnly] public CharacterStat.StatType StatName;
        public int Value;
    }
}