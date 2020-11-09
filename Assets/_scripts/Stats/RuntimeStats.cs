using Elysium.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class RuntimeStats : MonoBehaviour
{
    // ---------------- DETAILS ----------------    
    public virtual string Name => gameObject.name;

    public LootDetails Loot => loot;
    protected LootDetails loot;

    // ---------------- STATS ----------------
    public CharacterStat[] Stats => stats;
    protected CharacterStat[] stats;
    public Dictionary<CharacterStat.StatType, CharacterStat> StatsDictionary = new Dictionary<CharacterStat.StatType, CharacterStat>();

    public virtual int Strength => StatsDictionary[CharacterStat.StatType.STR].Value;
    public virtual int Agility => StatsDictionary[CharacterStat.StatType.AGI].Value;
    public virtual int Vitality => StatsDictionary[CharacterStat.StatType.VIT].Value;
    public virtual int Intelligence => StatsDictionary[CharacterStat.StatType.INT].Value;
    public virtual int Dexterity => StatsDictionary[CharacterStat.StatType.DEX].Value;
    public virtual int Luck => StatsDictionary[CharacterStat.StatType.LUK].Value;

    // ---------------- PARAMETERS ----------------
    public CharacterParameter[] Parameters => parameters;
    protected CharacterParameter[] parameters;
    public Dictionary<CharacterParameter.ParameterType, CharacterParameter> ParameterDictionary = new Dictionary<CharacterParameter.ParameterType, CharacterParameter>();

    public virtual int Damage => Strength;
    public virtual int Health => Vitality * 10;
    public virtual float AttackSpeed => 1 + (Agility / 10);

    // ---------------------------------------------

    public bool Initialized { get; private set; }
    public virtual void Init(CharacterStatWrapper[] statDetails, LootDetails loot)
    {
        this.loot = loot;
        InitializeStats(statDetails);
        InitializeParameters(statDetails);
        Initialized = true;
    }
    
    private void InitializeStats(CharacterStatWrapper[] statDetails)
    {
        this.stats = new CharacterStat[statDetails.Length];

        for (int i = 0; i < statDetails.Length; i++)
        {
            this.stats[i] = new CharacterStat(statDetails[i].StatName, statDetails[i].Value);
        }

        StatsDictionary = this.stats.ToDictionary((stat) => stat.StatName);
    }

    private void InitializeParameters(CharacterStatWrapper[] statDetails)
    {
        Array enumValues = Enum.GetValues(typeof(CharacterParameter.ParameterType));
        CharacterParameter.ParameterType[] parameterEnum = new CharacterParameter.ParameterType[enumValues.Length];
        Array.Copy(enumValues, parameterEnum, enumValues.Length);

        this.parameters = new CharacterParameter[enumValues.Length];
        Dictionary<CharacterParameter.ParameterType, Func<float>> ParameterSpecDictionary = InitializeParameterDictionary();

        for (int i = 0; i < parameters.Length; i++)
        {
            this.parameters[i] = new CharacterParameter(parameterEnum[i], ParameterSpecDictionary[parameterEnum[i]]);
        }

        ParameterDictionary = this.parameters.ToDictionary((parameter) => parameter.ParameterName);
    }

    private Dictionary<CharacterParameter.ParameterType, Func<float>> InitializeParameterDictionary()
    {
        Dictionary<CharacterParameter.ParameterType, Func<float>> ParameterSpecDictionary = new Dictionary<CharacterParameter.ParameterType, Func<float>>();

        ParameterSpecDictionary.Add(CharacterParameter.ParameterType.Damage, () => Damage);
        ParameterSpecDictionary.Add(CharacterParameter.ParameterType.Health, () => Health);
        ParameterSpecDictionary.Add(CharacterParameter.ParameterType.AttackSpeed, () => AttackSpeed);

        return ParameterSpecDictionary;
    }
}