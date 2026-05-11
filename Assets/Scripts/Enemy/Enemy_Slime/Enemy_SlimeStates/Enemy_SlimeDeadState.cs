using UnityEngine;

public class Enemy_SlimeDeadState : Enemy_DeadState
{
    private Enemy_Slime enemy_slime;


    public Enemy_SlimeDeadState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_slime = enemy as Enemy_Slime;
    }


    public override void Enter()
    {
        base.Enter();

        enemy_slime.CreateSlimeOnDeath();
    }
}
