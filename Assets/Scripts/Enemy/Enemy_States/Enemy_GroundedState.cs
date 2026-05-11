using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
    }


    public override void Update()
    {
        base.Update();

        if (enemy.PlayerDetected() == true)
            state_machine.ChangeState(enemy.battle_state);
    }
}
