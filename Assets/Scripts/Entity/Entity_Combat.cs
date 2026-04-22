using System;
using UnityEngine;
public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDamage;

    private Entity_SFX sfx;
    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData basic_attack_scale;


    [Header("Target detection")]
    [SerializeField] private Transform target_check;
    [SerializeField] private float target_check_radius = 1f;
    [SerializeField] private LayerMask what_is_target;


    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        sfx = GetComponent<Entity_SFX>();
        stats = GetComponent<Entity_Stats>();
    }


    public void PerformAttack()
    {
        bool target_got_hit = false;

        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;

            AttackData attack_data = stats.GetAttackData(basic_attack_scale);
            Entity_StatusHandler status_handler = target.GetComponent<Entity_StatusHandler>(); 
            
            float physical_damage = attack_data.physcial_damage;
            float elemental_damage = attack_data.elemental_damage;
            ElementType element = attack_data.element;

            target_got_hit = damageable.TakeDamage(physical_damage, elemental_damage, element, transform);
            
            if (element != ElementType.None)
                status_handler?.ApplyStatusEffect(element, attack_data.effect_data);

            if (target_got_hit)
            {
                OnDoingPhysicalDamage?.Invoke(physical_damage);
                vfx.CreateOnHitVfx(target.transform, attack_data.is_crit, element);
                sfx?.PlayAttackHit();
            }
        }

        if (target_got_hit == false)
            sfx?.PlayAttackMiss();
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
