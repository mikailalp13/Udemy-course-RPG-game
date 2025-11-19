using UnityEngine;
using System.Collections;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entity_vfx;
    private Entity_Stats entity_stats;
    private Entity_Health entity_health;
    private ElementType current_effect = ElementType.None;

    [Header("Electrify effect details")]
    [SerializeField] private GameObject lightning_strike_vfx;
    [SerializeField] private float current_charge;
    [SerializeField] private float maximum_charge = 1;
    private Coroutine electrify_co;



    private void Awake()
    {
        entity = GetComponent<Entity>();
        entity_health = GetComponent<Entity_Health>();
        entity_stats = GetComponent<Entity_Stats>();
        entity_vfx = GetComponent<Entity_VFX>();
    }

    public void ApplyElectrifyEffedt(float duration, float damage, float charge)
    {
        // builds up charge of electricty, if charges enough ? lightning strike : restart electirify status
        float lightning_resistance = entity_stats.GetElementalResistance(ElementType.Lightning);
        float final_charge = charge * (1 - lightning_resistance);
        current_charge = current_charge + final_charge;

        if (current_charge >= maximum_charge)
        {
            DoLightningStrike(damage);
            StopElectrifyEffect();
            return;
        }

        if (electrify_co != null)
            StopCoroutine(electrify_co);

        electrify_co = StartCoroutine(ElectrifyEffectCo(duration));
    }

    private void StopElectrifyEffect()
    {
        current_effect = ElementType.None;
        current_charge = 0;
        entity_vfx.StopAllVfx();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightning_strike_vfx, transform.position, Quaternion.identity);
        entity_health.ReduceHealth(damage);
    }

    private IEnumerator ElectrifyEffectCo(float duration)
    {
        current_effect = ElementType.Lightning;
        entity_vfx.PlayOnStatusVfx(duration, ElementType.Lightning);

        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();
    }

    public void ApplyBurnEffect(float duration, float fire_damage)
    {
        // gives damage over time
        float fire_resistance = entity_stats.GetElementalResistance(ElementType.Fire);
        float final_damage = fire_damage * (1 - fire_resistance);

        StartCoroutine(BurnEffectCo(duration, final_damage));
    }

    private IEnumerator BurnEffectCo(float duration, float total_damage)
    {
        current_effect = ElementType.Fire;
        entity_vfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int ticks_per_second = 2;
        int tick_count = Mathf.RoundToInt(ticks_per_second * duration);

        float damage_per_tick = total_damage / tick_count;
        float tick_interval = 1f / ticks_per_second;

        for (int i = 0; i < tick_count; i++)
        {
            entity_health.ReduceHealth(damage_per_tick);
            yield return new WaitForSeconds(tick_interval);
        }

        current_effect = ElementType.None;
    }


    public void ApplyChillEffect(float duration, float slow_multiplier)
    {
        float ice_resistance = entity_stats.GetElementalResistance(ElementType.Ice);
        float final_duration = duration * (1 - ice_resistance);

        StartCoroutine(ChillEffectCo(final_duration, slow_multiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slow_multiplier)
    {
        entity.SlowDownEntity(duration, slow_multiplier);
        current_effect = ElementType.Ice;
        entity_vfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        current_effect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && current_effect == ElementType.Lightning)
            return true;

        return current_effect == ElementType.None;
    }
}
