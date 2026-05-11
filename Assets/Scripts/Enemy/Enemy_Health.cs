using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy;
    private Player_QuestManager quest_manager;



    protected override void Start()
    {
        base.Start();

        enemy = GetComponent<Enemy>();
        quest_manager = Player.instance.quest_manager;
    }


    public override bool TakeDamage(float damage, float elemental_damage, ElementType element, Transform damage_dealer)
    {
        if (can_take_damage == false)
            return false;
            
        bool was_hit = base.TakeDamage(damage, elemental_damage, element, damage_dealer);

        if (was_hit == false)
            return false;

        if (damage_dealer.GetComponent<Player>() != null)
            enemy.TryEnterBattleState(damage_dealer);

        return true;
    }


    protected override void Die()
    {
        base.Die();

        quest_manager.AddProgress(enemy.quest_target_id);
    }
}
