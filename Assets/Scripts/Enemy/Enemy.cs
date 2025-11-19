using UnityEngine;
using System;
using System.Collections;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeadState deadState;
    public Enemy_StunnedState stunnedState;


    [Header("Battle details")]
    public float battle_move_speed = 3;
    public float attack_distance = 2;
    public float battle_time_duration = 5;
    public float min_retreat_distance = 1;
    public Vector2 retreat_velocity;


    [Header("Stunned state details")]
    public float stunned_duration = 1;
    public Vector2 stunned_velocity = new Vector2(7, 7);
    [SerializeField] protected bool can_be_stunned;


    [Header("Movement details")]
    public float idle_time = 2;
    public float move_speed = 1.4f;
    [Range(0, 2)]
    public float move_anim_speed_multiplier = 1;


    [Header("Player detection")]
    [SerializeField] private LayerMask what_is_player;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    public Transform player { get; private set; }


    protected override IEnumerator SlowDownEntityCo(float duration, float slow_multiplier)
    {
        float original_move_speed = move_speed;
        float original_battle_speed = battle_move_speed;
        float original_anim_speed = anim.speed;

        float speed_multiplier = 1 - slow_multiplier;

        move_speed = move_speed * slow_multiplier;
        battle_move_speed = battle_move_speed * slow_multiplier;
        anim.speed = anim.speed * slow_multiplier;

        yield return new WaitForSeconds(duration);

        move_speed = original_move_speed;
        battle_move_speed = original_battle_speed;
        anim.speed = original_anim_speed;
    }


    public void EnableCounterWindow(bool enable) => can_be_stunned = enable;

    public override void EntityDeath()
    {
        base.EntityDeath();

        state_machine.ChangeState(deadState);
    }

    private void HandlePlayerDeath()
    {
        state_machine.ChangeState(idleState);
    }

    public void TryEnterBattleState(Transform player)
    {
        if (state_machine.currentState == battleState || state_machine.currentState == attackState)
            return;

        this.player = player;
        state_machine.ChangeState(battleState);
    }
    
    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;
        return player;
    }

    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facing_dir, playerCheckDistance, what_is_player | what_is_ground);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facing_dir * playerCheckDistance), playerCheck.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facing_dir * attack_distance), playerCheck.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facing_dir * min_retreat_distance), playerCheck.position.y));
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
