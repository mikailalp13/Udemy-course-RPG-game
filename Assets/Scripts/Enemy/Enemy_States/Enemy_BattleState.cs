using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    protected Transform player;
    protected Transform last_target;
    protected float last_time_was_in_battle;
    protected float last_time_attacked = float.NegativeInfinity;


    public Enemy_BattleState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
    }


    public override void Enter()
    {
        base.Enter();

        UpdateBattleTimer();

        if (player == null)
            player = enemy.GetPlayerReference();
        //  player ??= enemy.GetPlayerReference(); // if player is null check, same with above


        if (ShouldRetreat())
            ShortRetreat();
    }


    protected void ShortRetreat()
    {
        float x = (enemy.retreat_velocity.x * enemy.active_slow_multiplier) * -DirectionToPlayer();
        float y = enemy.retreat_velocity.y;

        rb.linearVelocity = new Vector2(x, y);
        enemy.HandleFlip(DirectionToPlayer());
    }


    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected())
        {
            UpdateTargetIfNeeded();
            UpdateBattleTimer();
        }

        if (BattleTimeIsOver())
            state_machine.ChangeState(enemy.idle_state);
        
        if (WithinAttackRange() && enemy.PlayerDetected() && CanAttack())
        {
            last_time_attacked = Time.time;
            state_machine.ChangeState(enemy.attack_state);
        }
        else
        {
            float x_velocity = enemy.can_chase_player ? enemy.GetBattleMoveSpeed() : 0.0001f;

            if (enemy.ground_detected == false)
                x_velocity = 0.0001f;

            enemy.SetVelocity(x_velocity * DirectionToPlayer(), rb.linearVelocity.y);
        }
    }


    protected bool CanAttack() => Time.time > last_time_attacked + enemy.attack_cooldown;


    protected void UpdateTargetIfNeeded()
    {
        if (enemy.PlayerDetected() == false)
            return;
        
        Transform new_target = enemy.PlayerDetected().transform;

        if (new_target != last_target)
        {
            last_target = new_target;
            player = new_target;
        }
    }


    protected void UpdateBattleTimer() => last_time_was_in_battle = Time.time;

    protected bool BattleTimeIsOver() => Time.time > last_time_was_in_battle + enemy.battle_time_duration;

    protected bool WithinAttackRange() => DistanceToPlayer() < enemy.attack_distance;

    protected bool ShouldRetreat() => DistanceToPlayer() < enemy.min_retreat_distance;
    

    protected float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }


    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
