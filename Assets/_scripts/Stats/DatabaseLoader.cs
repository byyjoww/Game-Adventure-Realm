using Elysium.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DatabaseLoader : Singleton<DatabaseLoader>
{
    [SerializeField] private UnitStats baseStats;

    public static event Action OnDatabaseValueChanged;

    public static CharacterStatWrapper[] GetPlayerStats() => Instance.GetPlayerStatsFromDatabase();
    public static void WritePlayerStats(CharacterStatWrapper[] stats) => Instance.WriteStatsToDatabase(stats);
    public static int GetPlayerStatAllocationPoints() => Instance.GetPlayerStatAllocationPointsFromDatabase();
    public static void WritePlayerStatAllocationPoints(int variation) => Instance.WriteAllocationPointsToDatabase(variation);
    public static Progression GetPlayerProgress() => Instance.GetPlayerProgressFromDatabase();    

    #region STATS
    private CharacterStatWrapper[] GetPlayerStatsFromDatabase()
    {
        return baseStats.Stats;

        // TODO: GET STATS FROM DATABASE
        // TODO: SET STATS IN CLIENT
    }   

    public void WriteStatsToDatabase(CharacterStatWrapper[] stats)
    {
        int statAllocationCost = 0;
        foreach (var s in stats)
        {
            var dbStat = baseStats.Stats.Single(x => x.StatName == s.StatName);
            // Debug.Log($"calculating {s.Value} stat points in {dbStat.StatName}");
            statAllocationCost += s.Value;
        }

        if(statAllocationCost > allocation) { Debug.LogError("not enough stat allocation points"); return; }
        allocation -= statAllocationCost;

        foreach (var s in stats)
        {
            var dbStat = baseStats.Stats.Single(x => x.StatName == s.StatName);
            // Debug.Log($"allocating {s.Value} stat points in {dbStat.StatName}");
            dbStat.Value += s.Value;
        }
        
        OnDatabaseValueChanged?.Invoke();

        // TODO: WRITE TO DATABASE
        // TODO: UPDATE STATS IN CLIENT
    }

    int allocation = 0;
    private int GetPlayerStatAllocationPointsFromDatabase()
    {
        return allocation;
    }

    private void WriteAllocationPointsToDatabase(int variation)
    {
        allocation += variation;
        OnDatabaseValueChanged?.Invoke();

        // TODO: WRITE TO DATABASE
        // TODO: UPDATE STATS IN CLIENT
    }
    #endregion

    #region DETAILS

    private Progression GetPlayerProgressFromDatabase()
    {
        var expList = new int[10] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90 };
        return new Progression(1, 10, 0, expList);

        // TODO: GET LEVEL AND EXP FROM DATABASE
        // TODO: SET LEVEL AND EXP IN CLIENT
    }

    private void WriteProgressToDatabase(Progression progress)
    {
        throw new NotImplementedException();

        // TODO: WRITE TO DATABASE
        // TODO: UPDATE STATS IN CLIENT
    }
    #endregion
}
