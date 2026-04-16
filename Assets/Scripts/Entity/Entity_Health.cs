using System;
using UnityEngine;
using UnityEngine.UI;
public class Entity_Health : MonoBehaviour , IDamageable
{
    public event Action OnTakingDamage;

    private Slider health_bar;
    private Entity entity;
    private Entity_VFX entity_vfx;
    private Entity_Stats entity_stats;
    private Entity_DropManager drop_manager;

    [SerializeField] protected float current_health;

    
    [Header("Health Regen")]
    [SerializeField] private float regen_interval = 1;
    [SerializeField] private bool can_regenerate_health = true;
    public float last_damage_taken { get; private set; }
    public bool is_dead { get; private set; }
    protected bool can_take_damage = true;
    

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockback_power = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavy_knockback_power = new Vector2(4, 4);
    [SerializeField] private float knockback_duration = 0.2f;
    [SerializeField] private float heavy_knockback_duration = 0.5f;

    [Header("On Heavy Damage")]
    [SerializeField] private float heavy_damage_threshold = 0.3f; // percentage of health you should loose to heavy damage

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entity_vfx = GetComponent<Entity_VFX>();
        entity_stats = GetComponent<Entity_Stats>();
        health_bar = GetComponentInChildren<Slider>();
        drop_manager = GetComponent<Entity_DropManager>();

        SetupHealth();
    }

    private void SetupHealth()
    {
        if (entity_stats == null)
            return;
        
        current_health = entity_stats.GetMaxHealth();
        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regen_interval); // method name, delay time, repeat rate
    }

    public virtual bool TakeDamage(float damage, float elemental_damage, ElementType element, Transform damage_dealer)
    {
        if (is_dead || can_take_damage == false)
            return false;

        if (AttackEvaded())
            return false;

        Entity_Stats attacker_stats = damage_dealer.GetComponent<Entity_Stats>();

        float armor_reduction = attacker_stats != null ? attacker_stats.GetArmorReduction() : 0;
        float mitigation = entity_stats != null ? entity_stats.GetArmorMitigation(armor_reduction) : 0;
        float resistance = entity_stats != null ? entity_stats.GetElementalResistance(element) : 0;

        float physical_damage_taken = damage * (1 - mitigation);
        float elemental_damage_taken = elemental_damage * (1 - resistance);

        TakeKnockback(damage_dealer, physical_damage_taken);
        ReduceHealth(physical_damage_taken + elemental_damage_taken);

        last_damage_taken = physical_damage_taken + elemental_damage_taken;
        
        OnTakingDamage?.Invoke();
        return true;
    }
    
    public void SetCanTakeDamage(bool can_take_damage) => this.can_take_damage = can_take_damage;   

    private bool AttackEvaded()
    {
        if (entity_stats == null)
            return false;
        else
            return UnityEngine.Random.Range(0, 100) < entity_stats.GetEvasion();
    } 
        

    private void RegenerateHealth()
    {
        if (can_regenerate_health == false)
            return;
        
        float regen_amount = entity_stats.resources.health_regen.GetValue();
        IncreaseHealth(regen_amount);
    }

    public void IncreaseHealth(float heal_amount)
    {
        if (is_dead)
            return;
        
        float new_health = current_health + heal_amount;
        float max_health = entity_stats.GetMaxHealth();

        current_health = Mathf.Min(new_health, max_health); // with this method we stop the over healing
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        entity_vfx?.PlayeOnDamageVfx();
        current_health -= damage;
        UpdateHealthBar();

        if (current_health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        is_dead = true;
        entity?.EntityDeath();
        drop_manager?.DropItems();
    }

    public float GetHealthPercent() => current_health / entity_stats.GetMaxHealth();  

    public void SetHealthToPercent(float percent)
    {
        current_health = entity_stats.GetMaxHealth() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (health_bar == null)
            return;

        health_bar.value = current_health / entity_stats.GetMaxHealth();
    }

    private void TakeKnockback(Transform damage_dealer, float final_damage)
    {
        Vector2 knockback = CalculateKnockback(final_damage, damage_dealer);
        float duration = CalculateDuration(final_damage);

        entity?.RecieveKnockback(knockback, duration);
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavy_knockback_power : knockback_power;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavy_knockback_duration : knockback_duration;
    private bool IsHeavyDamage(float damage)
    {
        if (entity_stats == null)
            return false;
        else
            return damage / entity_stats.GetMaxHealth() > heavy_damage_threshold;
    } 
}
