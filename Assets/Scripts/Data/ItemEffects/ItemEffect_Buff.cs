using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item Effect / Buff Effect", fileName = "Item effect data - Buff")]

public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffs_to_apply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    private Player_Stats player_stats;

    public override bool CanBeUsed()
    {
        if (player_stats == null)
            player_stats = FindFirstObjectByType<Player_Stats>();

        if (player_stats.CanApplyBuffOf(source))
        {
            return true;
        }
        else
        {
            Debug.Log("Same buff effect can't be applied twice!");
            return false;
        }
    }

    public override void ExecuteEffect()
    {
        player_stats.ApplyBuff(buffs_to_apply, duration, source);
    }
}
