using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat combat;
    private bool countered_somebody;


    public Player_CounterAttackState(Player player, StateMachine state_machine, string anim_bool_name) : base(player, state_machine, anim_bool_name)
    {
        combat = player.GetComponent<Player_Combat>();
    }


    public override void Enter()
    {
        base.Enter();

        state_timer = combat.GetCounterRecoveryDuration();
        countered_somebody = combat.CounterAttackPerformed();
        anim.SetBool("counterAttackPerformed", countered_somebody);
    }
    

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.linearVelocity.y);

        if (trigger_called)
            state_machine.ChangeState(player.idleState);

        if (state_timer < 0 && countered_somebody == false)
            state_machine.ChangeState(player.idleState);
    }
}
