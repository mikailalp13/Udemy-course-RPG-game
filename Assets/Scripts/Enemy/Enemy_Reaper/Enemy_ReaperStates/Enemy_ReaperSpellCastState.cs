using UnityEngine;

public class Enemy_ReaperSpellCastState : EnemyState
{
    private Enemy_Reaper enemy_reaper;


    public Enemy_ReaperSpellCastState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_reaper = enemy as Enemy_Reaper;
    }


    public override void Enter()
    {
        base.Enter();

        enemy_reaper.SetVelocity(0, 0);
        enemy_reaper.SetSpellCastPerformed(false);
        enemy_reaper.SetSpellCastOnCooldown();
    }


    public override void Update()
    {
        base.Update();

        if (enemy_reaper.spell_cast_performed)
            anim.SetBool("spellCast_Performed", true);

        if (trigger_called)
        {
            if (enemy_reaper.ShouldTeleport())
                state_machine.ChangeState(enemy_reaper.reaper_teleport_state);
            else
                state_machine.ChangeState(enemy_reaper.reaper_battle_state);
        }
    }


    public override void Exit()
    {
        base.Exit();

        anim.SetBool("spellCast_Performed", false);
    }


}
