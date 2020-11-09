using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HealthController))]
public class UI_Health : MonoBehaviour
{
    private HealthController health;

    [SerializeField] GameObject onHitEffect;
    [SerializeField] GameObject onHealEffect;
    [SerializeField] GameObject damagePopup;

    public UnityEvent OnTakeDamage;

    private void Awake()
    {
        health = GetComponent<HealthController>();
    }

    private void Start()
    {
        health.OnTakeDamage += HandleDamage;
        health.OnHeal += HandleHeal;
    }

    private void OnDestroy()
    {
        health.OnTakeDamage -= HandleDamage;
        health.OnHeal -= HandleHeal;
    }

    private void HandleDamage(IDamageDealer dealer, int amount)
    {
        InstantiateDamagePopup(DamagePopup.DamagePopupType.Damage, amount);
        InstantiateEffect(onHitEffect);
        OnTakeDamage?.Invoke();
    }

    
    private void HandleHeal(IDamageDealer dealer, int amount)
    {
        InstantiateDamagePopup(DamagePopup.DamagePopupType.Heal, amount);
        InstantiateEffect(onHealEffect);
    }
    
    private void InstantiateEffect(GameObject prefab)
    {
        if (prefab == null) { return; }
        var effect = Instantiate(prefab, health.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        effect.transform.parent = health.transform;
    }

    private void InstantiateDamagePopup(DamagePopup.DamagePopupType damageType, int amount)
    {
        if (damagePopup == null) { return; }
        DamagePopup.Create(damagePopup, transform.position + new Vector3(0f, 1f, 0f), amount, damageType);
    }
}
