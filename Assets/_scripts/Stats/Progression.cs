using Elysium.Attributes;
using System;
using UnityEngine;

[System.Serializable]
public class Progression
{
    public int Level => level;
    [SerializeField, ReadOnly] private int level;
    public event Action<int> OnLevelGained;

    public int MaxLevel => maxLevel;
    [SerializeField, ReadOnly] private int maxLevel;
    public event Action<int> OnMaxLevelChanged;

    public int Experience => experience;
    [SerializeField, ReadOnly] private int experience;
    public event Action<int> OnCurrentExperienceChanged;

    [SerializeField, ReadOnly] private int[] experienceToNextLevelTable;
    public int ExperienceToNextLevel => experienceToNextLevelTable[Level];
    public event Action<int> OnRequiredExperienceChanged;

    public Progression(int level, int maxLevel, int experience, int[] experienceToNextLevel)
    {
        this.level = level;
        this.maxLevel = maxLevel;
        this.experience = experience;
        this.experienceToNextLevelTable = experienceToNextLevel;

        if (this.experienceToNextLevelTable.Length < this.maxLevel) { throw new System.Exception("max level is higher than experience required array"); }
    }

    public void GainExperience(int amount)
    {
        if (Level == maxLevel) { Debug.Log("unit is already at max level"); experience = 0; return; }

        experience += amount;
        OnCurrentExperienceChanged?.Invoke(Experience);
        CheckLevel();
    }

    public void CheckLevel()
    {
        while (experience >= ExperienceToNextLevel)
        {            
            experience -= ExperienceToNextLevel;
            GainLevel(1);

            if (Level == maxLevel) { Debug.Log("unit is at max level"); experience = 0; return; }
        }
    }

    public void GainLevel(int amount)
    {
        level += amount;
        OnLevelGained?.Invoke(Level);
        OnRequiredExperienceChanged?.Invoke(ExperienceToNextLevel);
        OnCurrentExperienceChanged?.Invoke(Experience);
    }

    public void SetMaxLevel(int maxLevel)
    {
        this.maxLevel = maxLevel;
        OnMaxLevelChanged?.Invoke(MaxLevel);
    }
}
