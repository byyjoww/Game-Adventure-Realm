using System;
using UnityEngine;
using Elysium.Attributes;

namespace Elysium.Stats
{
    public class CharacterParameter
    {
        public enum ParameterType { Damage = 0, Health = 1, AttackSpeed = 2 }
        public float Value => getValue() + valueFlatIncrease;
        private Func<float> getValue = () => 1;
        private float valueFlatIncrease = 0;

        public ParameterType ParameterName => parameterName;
        [SerializeField, ReadOnly] private ParameterType parameterName;

        public int BuffCount { get; private set; }
        public int DebuffCount { get; private set; }

        private const int MIN_VALUE = 1;
        private const int MAX_VALUE = 1000;

        public event Action OnBuffStatusChange;
        public event Action OnParameterIncrease;
        public event Action OnParameterChanged;

        public CharacterParameter(ParameterType parameterName, Func<float> getValue)
        {
            this.getValue = getValue;
            this.parameterName = parameterName;
            BuffCount = 0;
            DebuffCount = 0;
        }

        public void BuffFlat(float increaseAmount, float timeToIncreaseFor)
        {
            float newValue = valueFlatIncrease + increaseAmount;
            newValue = Mathf.Clamp(newValue, MIN_VALUE, MAX_VALUE);
            float actualIncreaseAmount = newValue - valueFlatIncrease;

            valueFlatIncrease += actualIncreaseAmount;
            BuffCount++;
            OnBuffStatusChange?.Invoke();
            OnParameterChanged?.Invoke();
            if (valueFlatIncrease > MAX_VALUE) { throw new System.Exception("value is higher than max value"); }

            Tools.DelayedExecution(() => { valueFlatIncrease -= actualIncreaseAmount; BuffCount--; OnBuffStatusChange?.Invoke(); OnParameterChanged?.Invoke(); }, timeToIncreaseFor);
        }

        public void DebuffFlat(float decreaseAmount, float timeToDecreaseFor)
        {
            float newValue = valueFlatIncrease - decreaseAmount;
            newValue = Mathf.Clamp(newValue, MIN_VALUE, MAX_VALUE);
            float actualDecreaseAmount = newValue + valueFlatIncrease;

            valueFlatIncrease -= actualDecreaseAmount;
            DebuffCount++;
            OnBuffStatusChange?.Invoke();
            OnParameterChanged?.Invoke();
            if (valueFlatIncrease < MIN_VALUE) { throw new System.Exception("value is lower than min value"); }

            Tools.DelayedExecution(() => { valueFlatIncrease += actualDecreaseAmount; DebuffCount--; OnBuffStatusChange?.Invoke(); OnParameterChanged?.Invoke(); }, timeToDecreaseFor);
        }

        public void BuffPercent(float increasePercent, float timeToIncreaseFor)
        {
            int flatIncrease = Mathf.RoundToInt(valueFlatIncrease * (increasePercent / 100));
            BuffFlat(flatIncrease, timeToIncreaseFor);
        }

        public void DebuffPercent(float decreasePercent, float timeToDecreaseFor)
        {
            int flatDecrease = Mathf.RoundToInt(valueFlatIncrease * (decreasePercent / 100));
            DebuffFlat(flatDecrease, timeToDecreaseFor);
        }
    }

    [System.Serializable]
    public class CharacterParameterWrapper
    {
        public CharacterParameterWrapper(CharacterParameter.ParameterType attribute, int amount)
        {
            this.AttributeName = attribute;
            Value = amount;
        }

        [ReadOnly] public CharacterParameter.ParameterType AttributeName;
        public int Value;
    }
}