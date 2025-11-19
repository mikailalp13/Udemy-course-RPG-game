using System.Runtime.Serialization;
using UnityEngine;
public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX vfx;
    private Entity_Stats stats;

    [Header("Target detection")]
    [SerializeField] private Transform target_check;
    [SerializeField] private float target_check_radius = 1f;
    [SerializeField] private LayerMask what_is_target;


    [Header("Status effect details")]
    [SerializeField] private float default_duration = 3f;
    [SerializeField] private float chill_slow_multiplier = 0.2f;
    [SerializeField] private float electrify_charge_build_up = 0.4f;
    [Space]
    [SerializeField] private float fire_scale = 0.8f;
    [SerializeField] private float lightning_scale = 2.5f;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }


    public void PerformAttack()
    {
        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;

            float elemental_damage = stats.GetElementalDamage(out ElementType element);
            float damage = stats.GetPhysicalDamage(out bool is_crit);

            bool target_got_hit = damageable.TakeDamage(damage, elemental_damage, element, transform);

            if (element != ElementType.None)
                ApplyStatusEffect(target.transform, element);


            if (target_got_hit)
            {
                vfx.UpdateOnHitColor(element);
                vfx.CreateOnHitVfx(target.transform, is_crit);
            }
        }
    }


    public void ApplyStatusEffect(Transform target, ElementType element, float scale_factor = 1)
    {
        Entity_StatusHandler status_handler = target.GetComponent<Entity_StatusHandler>();

        if (status_handler == null)
            return;

        if (element == ElementType.Ice && status_handler.CanBeApplied(ElementType.Ice))
            status_handler.ApplyChillEffect(default_duration, chill_slow_multiplier);
        else if (element == ElementType.Fire && status_handler.CanBeApplied(ElementType.Fire))
        {
            scale_factor = fire_scale;
            float fire_damage = stats.offense.fire_damage.GetValue() * scale_factor;
            status_handler.ApplyBurnEffect(default_duration, fire_damage);
        }
        else if (element == ElementType.Lightning && status_handler.CanBeApplied(ElementType.Lightning))
        {
            scale_factor = lightning_scale;
            float lightning_damage = stats.offense.lightning_damage.GetValue() * scale_factor;
            status_handler.ApplyElectrifyEffedt(default_duration, lightning_damage, electrify_charge_build_up);
        }
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(target_check.position, target_check_radius, what_is_target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target_check.position, target_check_radius);
    }
}
