using UnityEngine;

public class Player_CounterAttackState : PlayerState
{
    private Player_Combat combat;
    private bool counteredSomebody;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<Player_Combat>();
    }

    public override void Enter()
    {
        base.Enter();

        state_timer = combat.GetCounterRecoveryDuration();
        counteredSomebody = combat.CounterAttackPerformed();
        anim.SetBool("counterAttackPerformed", counteredSomebody);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0, rb.linearVelocity.y);

        if (trigger_called)
            stateMachine.ChangeState(player.idleState);

        if (state_timer < 0 && counteredSomebody == false)
            stateMachine.ChangeState(player.idleState);
    }
}
