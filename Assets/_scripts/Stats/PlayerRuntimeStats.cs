using Elysium.Attributes;
using Elysium.Save.Simple;
using Elysium.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerRuntimeStats : RuntimeStats
{
    [Header("Armory")]
    [SerializeField] private Armory armory;

    public Progression Progress => progress;
    [SerializeField, ReadOnly] private Progression progress;

    public int AllocationPoints => allocationPoints;
    [SerializeField, ReadOnly] private int allocationPoints;

    public override string Name => SaveSystem.LoadName("you");

    #region STATS
    public override int Strength => base.Strength + armory.GetStats(CharacterStat.StatType.STR);
    public override int Agility => base.Agility + armory.GetStats(CharacterStat.StatType.AGI);
    public override int Vitality => base.Vitality + armory.GetStats(CharacterStat.StatType.VIT);
    public override int Intelligence => base.Intelligence + armory.GetStats(CharacterStat.StatType.INT);
    public override int Dexterity => base.Dexterity + armory.GetStats(CharacterStat.StatType.DEX);
    public override int Luck => base.Luck + armory.GetStats(CharacterStat.StatType.LUK);
    #endregion

    #region PARAMETERS
    public override int Damage => base.Damage + armory.GetAttributes(CharacterParameter.ParameterType.Damage);
    public override int Health => base.Health + armory.GetAttributes(CharacterParameter.ParameterType.Health);
    public override float AttackSpeed => base.AttackSpeed + armory.GetAttributes(CharacterParameter.ParameterType.AttackSpeed);
    #endregion

    #region EVENT_BINDERS
    public event Action<string> OnNameChanged;
    public event Action<int> OnLevelGained;
    public event Action<int> OnCurrentExperienceChanged;
    public event Action<int> OnRequiredExperienceChanged;
    public event Action<int> OnStatAllocationPointsChanged;
    public event Action<CharacterStat.StatType, int> OnStatPointChanged;

    private void TargetRaiseOnNameChanged(string value) => OnNameChanged?.Invoke(value);
    private void TargetRaiseOnLevelGained(int value) => OnLevelGained?.Invoke(value);
    private void TargetRaiseOnCurrentExperienceChanged(int value) => OnCurrentExperienceChanged?.Invoke(value);
    private void TargetRaiseOnRequiredExperienceChanged(int value) => OnRequiredExperienceChanged?.Invoke(value);
    private void TargetRaiseOnStatAllocationPointsChanged(int value) => OnStatAllocationPointsChanged?.Invoke(value);
    private void TargetRaiseOnStatPointChanged(CharacterStat.StatType stat, int value) => OnStatPointChanged?.Invoke(stat, value);
    #endregion

    private void Start() => Init(DatabaseLoader.GetPlayerStats(), new LootDetails(0, 0, null), DatabaseLoader.GetPlayerProgress(), DatabaseLoader.GetPlayerStatAllocationPoints());
    protected void OnDestroy() => UnbindEvents();

    public virtual void Init(CharacterStatWrapper[] stats, LootDetails loot, Progression progress, int allocationPoints)
    {
        base.Init(stats, loot);
        this.allocationPoints = allocationPoints;
        this.progress = progress;

        BindEvents();        
    }

    public void BindEvents()
    {
        DatabaseLoader.OnDatabaseValueChanged += FetchFromDatabase;
        progress.OnLevelGained += GetStatAllocationPoint;

        Progress.OnLevelGained += TargetRaiseOnLevelGained;
        Progress.OnCurrentExperienceChanged += TargetRaiseOnCurrentExperienceChanged;
        Progress.OnRequiredExperienceChanged += TargetRaiseOnRequiredExperienceChanged;        
        foreach (var stat in Stats) { stat.OnStatIncrease += TargetRaiseOnStatPointChanged; }
    }

    public void UnbindEvents()
    {
        DatabaseLoader.OnDatabaseValueChanged -= FetchFromDatabase;
        progress.OnLevelGained -= GetStatAllocationPoint;

        Progress.OnLevelGained += TargetRaiseOnLevelGained;
        Progress.OnCurrentExperienceChanged += TargetRaiseOnCurrentExperienceChanged;
        Progress.OnRequiredExperienceChanged += TargetRaiseOnRequiredExperienceChanged;
        foreach (var stat in Stats) { stat.OnStatIncrease += TargetRaiseOnStatPointChanged; }
    }

    public void GetStatAllocationPoint(int currentLevel)
    {
        DatabaseLoader.WritePlayerStatAllocationPoints(1);
    }

    public void SetAllocationPoints(int amount) 
    {        
        allocationPoints = amount;
        TargetRaiseOnStatAllocationPointsChanged(amount);
    }

    public void ChangeName(string newName)
    {
        SaveSystem.SaveName(newName);
        TargetRaiseOnNameChanged(newName);
    }

    private void FetchFromDatabase()
    {
        var s = DatabaseLoader.GetPlayerStats();
        var d = DatabaseLoader.GetPlayerProgress();
        var a = DatabaseLoader.GetPlayerStatAllocationPoints();

        for (int i = 0; i < s.Length; i++) { Stats[i].Increase(s[i].Value - Stats[i].Value); }

        SetAllocationPoints(a);
    }
}
