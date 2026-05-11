using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
    }


    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
    }
    

    public override void Update()
    {
        base.Update();

        if (trigger_called)
            state_machine.ChangeState(enemy.battle_state);
    }
}
