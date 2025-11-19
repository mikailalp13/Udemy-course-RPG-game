using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    public bool CanBeCountered { get => can_be_stunned; }
    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, state_machine, "idle");
        moveState = new Enemy_MoveState(this, state_machine, "move");
        attackState = new Enemy_AttackState(this, state_machine, "attack");
        battleState = new Enemy_BattleState(this, state_machine, "battle");
        deadState = new Enemy_DeadState(this, state_machine, "idle");
        stunnedState = new Enemy_StunnedState(this, state_machine, "stunned");
    }

    protected override void Start()
    {
        base.Start();

        state_machine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        state_machine.ChangeState(stunnedState);
    }
}
