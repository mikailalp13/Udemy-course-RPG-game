using UnityEngine;

public class Enemy_ReaperBattleState : Enemy_BattleState
{
    private Enemy_Reaper enemy_reaper;


    public Enemy_ReaperBattleState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_reaper = enemy as Enemy_Reaper;
    }


    public override void Enter()
    {
        base.Enter();

        state_timer = enemy_reaper.max_battle_idle_time;
    }


    public override void Update()
    {
        // boss won't go back to idle

        state_timer -= Time.deltaTime;
        UpdateAnimationParameters();

        if (state_timer < 0)
            state_machine.ChangeState(enemy_reaper.reaper_teleport_state);

        if (enemy.PlayerDetected())
            UpdateTargetIfNeeded();
        
        if (WithinAttackRange() && enemy.PlayerDetected() && CanAttack())
        {
            last_time_attacked = Time.time;
            state_machine.ChangeState(enemy_reaper.reaper_attack_state);
        }
        else
        {
            float x_velocity = enemy.can_chase_player ? enemy.GetBattleMoveSpeed() : 0.0001f;

            if (enemy.ground_detected == false)
                x_velocity = 0.00001f;

            enemy.SetVelocity(x_velocity * DirectionToPlayer(), rb.linearVelocity.y);
        }
    }
}
