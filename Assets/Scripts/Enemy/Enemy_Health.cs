using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();

    public override bool TakeDamage(float damage, float elemental_damage, ElementType element, Transform damage_dealer)
    {
        bool was_hit = base.TakeDamage(damage, elemental_damage, element, damage_dealer);

        if (was_hit == false)
            return false;

        if (damage_dealer.GetComponent<Player>() != null)
            enemy.TryEnterBattleState(damage_dealer);

        return true;
    }
}
