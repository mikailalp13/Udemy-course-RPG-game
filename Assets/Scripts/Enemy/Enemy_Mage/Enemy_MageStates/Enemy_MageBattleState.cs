using UnityEngine;

public class Enemy_MageBattleState : Enemy_BattleState
{
    private Enemy_Mage enemy_mage;
    private float last_time_used_retreat = float.NegativeInfinity;



    public Enemy_MageBattleState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_mage = enemy as Enemy_Mage;
    }


    public override void Enter()
    {
        base.Enter();

        if (ShouldRetreat())
        {
            if (CanUseRetreatAbility())
                Retreat();
            else 
                ShortRetreat();
        }
    }


    private void Retreat()
    {
        last_time_used_retreat = Time.time;

        state_machine.ChangeState(enemy_mage.mage_retreat_state);
    }


    private bool CanUseRetreatAbility() => Time.time > last_time_used_retreat + enemy_mage.retreat_cooldown;
}
