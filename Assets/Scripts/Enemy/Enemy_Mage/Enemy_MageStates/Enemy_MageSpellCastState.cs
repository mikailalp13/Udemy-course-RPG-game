using UnityEngine;

public class Enemy_MageSpellCastState : EnemyState
{
    private Enemy_Mage enemy_mage;


    public Enemy_MageSpellCastState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_mage = enemy as Enemy_Mage;
    }


    public override void Enter()
    {
        base.Enter();

        enemy_mage.SetVelocity(0, 0);
        enemy_mage.SetSpellCastPerformed(false);
    }


    public override void Update()
    {
        base.Update();

        if (enemy_mage.spell_cast_performed)
            anim.SetBool("spellCastPerformed", true);

        if (trigger_called)
            state_machine.ChangeState(enemy.battle_state);
    }


    public override void Exit()
    {
        base.Exit();

        anim.SetBool("spellCastPerformed", false);
    }
}
