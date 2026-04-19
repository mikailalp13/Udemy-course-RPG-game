using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup / Item Data / Item Effect / Buff Effect", fileName = "Item effect data - Buff")]

public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buffs_to_apply;
    [SerializeField] private float duration;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    public override bool CanBeUsed(Player player)
    {
        if (player.stats.CanApplyBuffOf(source))
        {
            this.player = player;
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
        player.stats.ApplyBuff(buffs_to_apply, duration, source);
        player = null;
    }
}
