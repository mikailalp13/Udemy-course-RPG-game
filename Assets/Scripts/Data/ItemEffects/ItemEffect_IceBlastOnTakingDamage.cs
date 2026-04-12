using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item Effect / Ice Blast", fileName = "Item effect data - Ice Blast On Taking Damage")]

public class ItemEffect_IceBlastOnTakingDamage : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effect_data;
    [SerializeField] private float ice_damage;
    [SerializeField] private LayerMask what_is_enemy;

    [Space]
    [SerializeField] private float health_percent_trigger = 0.25f;
    [SerializeField] private float cooldown;
    private float last_time_used = -999;

    [Header("VFX Objects")]
    [SerializeField] private GameObject ice_blast_vfx;
    [SerializeField] private GameObject on_hit_vfx;


    public override void ExecuteEffect()
    {
        bool no_cooldown = Time.time >= last_time_used + cooldown;
        bool reached_threshold = player.health.GetHealthPercent() <= health_percent_trigger;

        if (no_cooldown && reached_threshold)
        {
            player.vfx.CreateEffectOf(ice_blast_vfx, player.transform);
            last_time_used = Time.time;
            DamageEnemiesWithIce();
        }
    }

    private void DamageEnemiesWithIce()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, what_is_enemy);

        foreach (var target in enemies)
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null)
                continue;
            
            bool target_got_hit = damageable.TakeDamage(0, ice_damage, ElementType.Ice, player.transform);

            Entity_StatusHandler status_handler = target.GetComponent<Entity_StatusHandler>();
            status_handler?.ApplyStatusEffect(ElementType.Ice, effect_data);

            if (target_got_hit)
                player.vfx.CreateEffectOf(on_hit_vfx, target.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);

        player.health.OnTakingDamage += ExecuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        player.health.OnTakingDamage -= ExecuteEffect;
        player = null;
    }
}
