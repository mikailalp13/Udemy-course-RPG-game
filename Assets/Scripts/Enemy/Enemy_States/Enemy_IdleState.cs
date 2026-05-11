using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
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
            state_machine.ChangeState(enemy.move_state);
    }
}
