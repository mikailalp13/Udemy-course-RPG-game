using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item Effect / Life Steal Effect", fileName = "Item effect data - Life Steal")]

public class ItemEffect_LifeSteal : ItemEffectDataSO
{
    [SerializeField] private float percent_healed_on_attack = 0.2f;


    public override void Subscribe(Player player)
    {
        base.Subscribe(player);

        player.combat.OnDoingPhysicalDamage += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();

        player.combat.OnDoingPhysicalDamage -= HealOnDoingDamage;
        player = null;
    }

    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * percent_healed_on_attack);
    }
}
