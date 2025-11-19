using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static event Action OnPlayerDeath;
    public PlayerInputSet input { get; private set; }
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public Player_WallJumpState wallJumpState { get; private set; }
    public Player_DashState dashState { get; private set; }
    public Player_BasicAttackState basicAttackState { get; private set; }
    public Player_JumpAttackState jumpAttackState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CounterAttackState counterAttackState { get; private set; }


    [Header("Attack details")]
    public Vector2[] attack_velocity;
    public Vector2 jump_attack_velocity;
    public float attack_velocity_duration = 0.1f;
    public float combo_reset_time = 1;
    private Coroutine queued_attack_co;


    [Header("Movement details")]
    public float move_speed;
    public float jump_force = 5;
    public Vector2 wall_jump_force;

    [Range(0, 1)]
    public float in_air_move_multiplier = 0.7f; // should be from 0 to 1
    [Range(0, 1)]
    public float wall_slide_slow_multiplier = 0.7f;
    [Space]
    public float dash_duration = 0.25f;
    public float dash_speed = 20;
    public Vector2 move_input { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, state_machine, "idle");
        moveState = new Player_MoveState(this, state_machine, "move");
        jumpState = new Player_JumpState(this, state_machine, "jumpFall");
        fallState = new Player_FallState(this, state_machine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, state_machine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, state_machine, "jumpFall");
        dashState = new Player_DashState(this, state_machine, "dash");
        basicAttackState = new Player_BasicAttackState(this, state_machine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, state_machine, "jumpAttack");
        deadState = new Player_DeadState(this, state_machine, "dead");
        counterAttackState = new Player_CounterAttackState(this, state_machine, "counterAttack");
    }

    protected override void Start()
    {
        base.Start();

        state_machine.Initialize(idleState);
    }


    protected override IEnumerator SlowDownEntityCo(float duration, float slow_multiplier)
    {
        float original_move_speed = move_speed;
        float original_jump_force = jump_force;
        float original_anim_speed = anim.speed;
        Vector2 original_wall_jump = wall_jump_force;
        Vector2 original_jump_attack = jump_attack_velocity;
        Vector2[] original_attack_velocity = new Vector2[attack_velocity.Length];
        Array.Copy(attack_velocity, original_attack_velocity, attack_velocity.Length); 
        // if we don't do this and just try to use this "Vector2[] original_attack_velocity = attack_velocity.Length;" we cant go back to the original array later on because in this case we dont copy the values we just point them

        float speed_multiplier = 1 - slow_multiplier;

        move_speed = move_speed * speed_multiplier;
        jump_force = jump_force * speed_multiplier;
        anim.speed = anim.speed * speed_multiplier;
        wall_jump_force = wall_jump_force * speed_multiplier;
        jump_attack_velocity = jump_attack_velocity * speed_multiplier;


        for (int i = 0; i < attack_velocity.Length; i++)
        {
            attack_velocity[i] = attack_velocity[i] * speed_multiplier;
        }

        yield return new WaitForSeconds(duration);

        move_speed = original_move_speed;
        jump_force = original_jump_force;
        anim.speed = original_anim_speed;
        wall_jump_force = original_wall_jump;
        jump_attack_velocity = original_jump_attack;

        for (int i = 0; i < attack_velocity.Length; i++)
        {
            attack_velocity[i] = original_attack_velocity[i];
        }
        

    }


    public override void EntityDeath()
    {
        base.EntityDeath();

        OnPlayerDeath?.Invoke();
        state_machine.ChangeState(deadState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queued_attack_co != null)
            StopCoroutine(queued_attack_co);
        queued_attack_co = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        state_machine.ChangeState(basicAttackState);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Movement.performed += ctx => move_input = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => move_input = Vector2.zero;
    }
    private void OnDisable()
    {
        input.Disable();
    }
}
