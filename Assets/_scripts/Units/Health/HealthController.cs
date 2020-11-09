using Elysium.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HealthController : MonoBehaviour, IFillable, IDamageable
{
    private Func<float> getMax = () => 1;
    [SerializeField, ReadOnly] private int max = 1;
    [SerializeField, ReadOnly] private int current = 1;
    [SerializeField] private DamageTeam damageTeam;
    [SerializeField, ReadOnly] private bool isDead;

    public int Max
    {
        get
        {
            SetMaxHealth();
            return max;
        }
    }

    public int Current => current;

    public float MaxFill => Max;
    public float CurrentFill => Current;

    public DamageTeam Team => damageTeam;
    public bool IsDead => isDead;
    public GameObject DamageableObject => gameObject;

    public List<DamageRecord> damageDealers;
    public class DamageRecord
    {
        public IDamageDealer damageDealer;
        public int amount;

        public DamageRecord(IDamageDealer _damageDealer, int _amount)
        {
            damageDealer = _damageDealer;
            amount = _amount;
        }
    }

    public IDamageDealer GetKiller() => damageDealers.Where(x => x.damageDealer != null).OrderBy(x => x.amount).ToList()[0].damageDealer;

    // EVENTS
    public event Action OnHealthEmpty;
    public event Action OnFillValueChanged;
    public event Action<IDamageDealer, int> OnTakeDamage;
    public event Action<IDamageDealer, int> OnHeal;
    public event Action<int, int> OnChanged;
    public event Action OnDeathStatusChange;
    public UnityEvent OnDeath;

    private void RaiseOnFillValueChanged(int oldValue, int newValue) => OnFillValueChanged?.Invoke();
    private int SetMaxHealth() => max = (int)getMax();
    private void Start() => damageDealers = new List<DamageRecord>();

    #region API
    public void BindMaxHealth(Func<float> lambda)
    {
        getMax = lambda;
        Fill();
    }
    
    public void TakeDamage(IDamageDealer damageDealer, int amount)
    {
        if (IsDead) { Debug.Log($"{gameObject.name} is dead"); return; }

        int prev = current;
        current = Mathf.Clamp(current - amount, 0, Max);
        Debug.Log($"Takes {amount} damage | Before: {prev} | After: {current}.");

        damageDealers.Add(new DamageRecord(damageDealer, amount));
        OnTakeDamage?.Invoke(damageDealer, amount);
        OnChanged?.Invoke(prev, current);
        RaiseOnFillValueChanged(prev, current);
        CheckDeathStatus();
    }

    public void Heal(IDamageDealer damageDealer, int amount)
    {
        if (IsDead) { Debug.Log($"{gameObject.name} is dead"); return; }

        int prev = current;
        current = Mathf.Clamp(current + amount, 0, Max);
        Debug.Log($"Heals {amount} health | Before: {prev} | After: {current}.");

        OnHeal?.Invoke(damageDealer, amount);
        OnChanged?.Invoke(prev, current);
        RaiseOnFillValueChanged(prev, current);
        CheckDeathStatus();
    }

    public void Fill()
    {
        if (isDead) { throw new System.Exception("tried to fill health while character is dead"); }

        int prev = current;
        current = Max;
        RaiseOnFillValueChanged(prev, current);
    }

    public void Ressurect()
    {
        if (!IsDead) { Debug.Log($"{gameObject.name} is not dead"); return; }

        isDead = false;
        Fill();

        OnDeathStatusChange?.Invoke();
    }

    private void CheckDeathStatus()
    {        
        if (current <= 0)
        {
            current = 0;
            OnHealthEmpty?.Invoke();
            Die();
        }
    }

    private void Die()
    {
        if (IsDead) { Debug.Log($"{gameObject.name} is already dead"); return; }

        isDead = true;
        OnDeath?.Invoke();
        OnDeathStatusChange?.Invoke();
    }    
    #endregion
}