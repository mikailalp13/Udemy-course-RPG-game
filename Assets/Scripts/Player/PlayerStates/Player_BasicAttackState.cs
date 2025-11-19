using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attack_velocity_timer;
    private float last_time_attacked;

    private bool combo_attack_queued;
    private int attack_dir;
    private int combo_index = 1;
    private int combo_limit = 3;
    private const int FirstComboIndex = 1; // the combo index starts with 1, this parameter is used in the Animator.


    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (combo_limit != player.attack_velocity.Length)
        {
            Debug.LogWarning("Adjusted combo limit to match attack velocity array!");
            combo_limit = player.attack_velocity.Length;
        }
            
    }

    public override void Enter()
    {
        base.Enter();
        combo_attack_queued = false;
        ResetComboIndexIfNeeded();
        SyncAttackSpeed();

        // define attack direction according to input
        attack_dir = player.move_input.x != 0 ? ((int)player.move_input.x) : player.facing_dir;

        anim.SetInteger("basicAttackIndex", combo_index);
        ApplyAttackVelocity();
    }


    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if (triggerCalled)
            HandleStateExit();            
    }

    public override void Exit()
    {
        base.Exit();
        combo_index++;
        last_time_attacked = Time.time;
    }

    private void HandleStateExit()
    {
        if (combo_attack_queued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    private void QueueNextAttack()
    {
        if (combo_index < combo_limit)
            combo_attack_queued = true;
    }

    private void HandleAttackVelocity()
    {
        attack_velocity_timer -= Time.deltaTime;

        if (attack_velocity_timer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attack_velocity[combo_index - 1];

        attack_velocity_timer = player.attack_velocity_duration;
        player.SetVelocity(attackVelocity.x * attack_dir, attackVelocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > last_time_attacked + player.combo_reset_time)
            combo_index = FirstComboIndex;
              
        if (combo_index > combo_limit)
            combo_index = FirstComboIndex;
    }
}
