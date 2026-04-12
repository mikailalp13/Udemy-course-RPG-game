using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item Effect / Heal Effect", fileName = "Item effect data - Heal")]

public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float heal_percent = 0.1f;
    public override void ExecuteEffect()
    {
        Player player = FindFirstObjectByType<Player>();

        float heal_amount = player.stats.GetMaxHealth() * heal_percent;

        player.health.IncreaseHealth(heal_amount);
    }
}
