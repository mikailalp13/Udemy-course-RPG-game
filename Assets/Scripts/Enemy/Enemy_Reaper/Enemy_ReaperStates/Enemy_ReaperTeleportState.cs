using UnityEngine;

public class Enemy_ReaperTeleportState : EnemyState
{
    private Enemy_Reaper enemy_reaper;


    public Enemy_ReaperTeleportState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_reaper = enemy as Enemy_Reaper;
    }


    public override void Enter()
    {
        base.Enter();

        enemy_reaper.MakeUntargetable(true);
    }


    public override void Update()
    {
        base.Update();

        if (enemy_reaper.teleport_trigger)
        {
            enemy_reaper.transform.position = enemy_reaper.FindTeleportPoint();
            enemy_reaper.SetTeleportTrigger(false);
        }

        if (trigger_called)
        {
            if (enemy_reaper.CanDoSpellCast())
                state_machine.ChangeState(enemy_reaper.reaper_spell_cast_state);
            else
                state_machine.ChangeState(enemy_reaper.reaper_battle_state);
        }    
    }


    public override void Exit()
    {
        base.Exit();

        enemy_reaper.MakeUntargetable(false);
    }
}

