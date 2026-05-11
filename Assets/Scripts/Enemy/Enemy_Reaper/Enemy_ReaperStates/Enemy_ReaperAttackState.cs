using UnityEngine;

public class Enemy_ReaperAttackState : EnemyState
{
    private Enemy_Reaper enemy_reaper;


    public Enemy_ReaperAttackState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_reaper = enemy as Enemy_Reaper;
    }


    public override void Enter()
    {
        base.Enter();

        SyncAttackSpeed();
    }


    public override void Update()
    {
        base.Update();

        if (trigger_called)
        {
            if (enemy_reaper.ShouldTeleport())
                state_machine.ChangeState(enemy_reaper.reaper_teleport_state);
            else
                state_machine.ChangeState(enemy_reaper.reaper_battle_state);
        }
    }
}
