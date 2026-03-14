using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private Transform player;
    private Transform last_target;
    private float lastTimeWasInBattle;
    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
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
        {
            rb.linearVelocity = new Vector2((enemy.retreat_velocity.x * enemy.active_slow_multiplier) * -DirectionToPlayer(), enemy.retreat_velocity.y);
            enemy.HandleFlip(DirectionToPlayer());
        }

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
            stateMachine.ChangeState(enemy.idleState);
        
        if (WithinAttackRange() && enemy.PlayerDetected())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.GetBattleMoveSpeed() * DirectionToPlayer(), rb.linearVelocity.y);
    }

    private void UpdateTargetIfNeeded()
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

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;

    private bool BattleTimeIsOver() => Time.time > lastTimeWasInBattle + enemy.battle_time_duration;

    private bool WithinAttackRange() => DistanceToPlayer() < enemy.attack_distance;

    private bool ShouldRetreat() => DistanceToPlayer() < enemy.min_retreat_distance;
    
    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemy.transform.position.x);
    }

    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
