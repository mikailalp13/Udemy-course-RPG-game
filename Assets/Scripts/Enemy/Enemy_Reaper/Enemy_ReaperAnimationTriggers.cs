using UnityEngine;

public class Enemy_ReaperAnimationTriggers : Enemy_AnimationTriggers
{
    private Enemy_Reaper enemy_reaper;


    protected override void Awake()
    {
        base.Awake();

        enemy_reaper = GetComponentInParent<Enemy_Reaper>();
    }


    private void TeleportTrigger()
    {
        enemy_reaper.SetTeleportTrigger(true);
    }
}
