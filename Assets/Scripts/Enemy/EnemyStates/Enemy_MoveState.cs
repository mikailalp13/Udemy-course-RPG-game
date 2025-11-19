using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine state_machine, string animBoolName) : base(enemy, state_machine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (enemy.ground_detected == false || enemy.wall_detected)
            enemy.Flip();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.move_speed * enemy.facing_dir, rb.linearVelocity.y);

        if (enemy.ground_detected == false || enemy.wall_detected)
            stateMachine.ChangeState(enemy.idleState);

    }
}
