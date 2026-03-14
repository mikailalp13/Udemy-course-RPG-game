using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject on_hit_vfx;
    [Space]
    [SerializeField] protected LayerMask what_is_enemy;
    [SerializeField] protected Transform target_check;
    [SerializeField] protected float check_radius = 1f;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Entity_Stats player_stats;
    protected DamageScaleData damage_scale_data;
    protected ElementType used_element;
    protected bool target_got_hit;
    protected Transform last_target;


    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in GetEnemiesAround(t, radius))
        {
            IDamageable damagable = target.GetComponent<IDamageable>();

            if (damagable == null)
                continue;
            
            AttackData attack_data = player_stats.GetAttackData(damage_scale_data);
            Entity_StatusHandler status_handler = target.GetComponent<Entity_StatusHandler>();

            float phys_damage = attack_data.physcial_damage;
            float elem_damage = attack_data.elemental_damage;
            ElementType element = attack_data.element;

            target_got_hit = damagable.TakeDamage(phys_damage, elem_damage, element, transform);

            if (element != ElementType.None)
                status_handler?.ApplyStatusEffect(element, attack_data.effect_data);

            if (target_got_hit)
            {
                last_target = target.transform;
                Instantiate(on_hit_vfx, target.transform.position, Quaternion.identity);
            }
            
            used_element = element;
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closest_distance = Mathf.Infinity;

        foreach (var enemy in GetEnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);  

            if (distance < closest_distance)
            {
                target = enemy.transform; 
                closest_distance = distance;
            }
        }

        return target;
    }

    protected Collider2D[] GetEnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, what_is_enemy); 
    }

    protected virtual void OnDrawGizmos()
    {
        if (target_check == null)
            target_check = transform;

        Gizmos.DrawWireSphere(target_check.position, check_radius);
    }
}
