using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsBinder : MonoBehaviour
{
    private PlayerRuntimeStats stats;

    // Binding Components
    private HealthController health;
    private AttackController attack;

    [Header("UI Values")]
    [SerializeField] private IntValue currentLevel;
    [SerializeField] private IntValue maxLevel;
    [SerializeField] private IntValue currentEXP;
    [SerializeField] private IntValue maxEXP;

    private void Awake()
    {
        stats = GetComponent<PlayerRuntimeStats>();
        health = GetComponentInChildren<HealthController>();
        attack = GetComponentInChildren<AttackController>();
    }

    private void Start() => StartCoroutine(BindAllEvents());

    private IEnumerator BindAllEvents()
    {
        yield return new WaitUntil(() => stats.Initialized);

        BindLevelUpEffect();
        BindUIValues();

        yield return null;
    }

    private void BindLevelUpEffect() => stats.Progress.OnLevelGained += (_level) => health.Fill();

    public void BindUIValues()
    {
        stats.Progress.OnLevelGained += (_level) => currentLevel.Value = _level;
        stats.Progress.OnMaxLevelChanged += (_level) => maxLevel.Value = _level;
        stats.Progress.OnCurrentExperienceChanged += (_experience) => currentEXP.Value = _experience;
        stats.Progress.OnRequiredExperienceChanged += (_experience) => maxEXP.Value = _experience;

        currentLevel.Value = stats.Progress.Level;
        maxLevel.Value = stats.Progress.MaxLevel;
        currentEXP.Value = stats.Progress.Experience;
        maxEXP.Value = stats.Progress.ExperienceToNextLevel;
    }
}
