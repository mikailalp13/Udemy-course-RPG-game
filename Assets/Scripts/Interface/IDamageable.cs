using UnityEngine;

public interface IDamageable 
{
    public bool TakeDamage(float damage, float elemental_damage, ElementType element, Transform damage_dealer);
}
