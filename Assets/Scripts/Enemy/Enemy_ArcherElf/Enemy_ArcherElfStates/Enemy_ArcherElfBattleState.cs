using UnityEngine;

public class Enemy_ArcherElfBattleState : Enemy_BattleState
{
    private bool can_flip;
    private bool reached_dead_end;


    public Enemy_ArcherElfBattleState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
    }


    public override void Enter()
    {
        base.Enter();
        reached_dead_end = false;
    }


    public override void Update()
    {
        state_timer -= Time.deltaTime;
        UpdateAnimationParameters();

        if (enemy.ground_detected == false || enemy.wall_detected)
            reached_dead_end = true;

        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }


        if (BattleTimeIsOver())
            state_machine.ChangeState(enemy.idle_state);
        

        if (CanAttack())
        {
            if (enemy.PlayerDetected() == false && can_flip)
            {
                enemy.HandleFlip(DirectionToPlayer());
                can_flip = false;
            }
            
            enemy.SetVelocity(0, rb.linearVelocity.y);

            if (WithinAttackRange() && enemy.PlayerDetected())
            {
                can_flip = true;
                last_time_attacked = Time.time;
                state_machine.ChangeState(enemy.attack_state);
            }
        }
        else
        {
            bool should_walk_away = reached_dead_end == false && DistanceToPlayer() < (enemy.attack_distance * 0.85f);

            // Archer walks away from the player
            if (should_walk_away)
                enemy.SetVelocity((enemy.GetBattleMoveSpeed() * -1) * DirectionToPlayer(), rb.linearVelocity.y);
            else
            {
                enemy.SetVelocity(0, rb.linearVelocity.y);

                if (enemy.PlayerDetected() == false)
                    enemy.HandleFlip(DirectionToPlayer());
            }
        }
    }
}
