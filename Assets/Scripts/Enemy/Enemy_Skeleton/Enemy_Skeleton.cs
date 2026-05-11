using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterable
{
    public bool CanBeCountered { get => can_be_stunned; }


    protected override void Awake()
    {
        base.Awake();

        idle_state = new Enemy_IdleState(this, state_machine, "idle");
        move_state = new Enemy_MoveState(this, state_machine, "move");
        attack_state = new Enemy_AttackState(this, state_machine, "attack");
        battle_state = new Enemy_BattleState(this, state_machine, "battle");
        dead_state = new Enemy_DeadState(this, state_machine, "idle");
        stunned_state = new Enemy_StunnedState(this, state_machine, "stunned");
    }


    protected override void Start()
    {
        base.Start();

        state_machine.Initialize(idle_state);
    }


    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        state_machine.ChangeState(stunned_state);
    }
}
