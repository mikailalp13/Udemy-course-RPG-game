using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        state_timer = enemy.idle_time;
    }

    public override void Update()
    {
        base.Update();

        if (state_timer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
