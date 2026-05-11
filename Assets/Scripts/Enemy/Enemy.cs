using System;
using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
    [Header("Quest Info")]
    public string quest_target_id;

    
    public Entity_VFX vfx { get; private set; }
    public Entity_Stats stats { get; private set; }
    public Enemy_Health health { get; private set; }
    public Entity_Combat combat { get; private set; }

    public Enemy_DeadState dead_state;
    public Enemy_IdleState idle_state;
    public Enemy_MoveState move_state;
    public Enemy_AttackState attack_state;
    public Enemy_BattleState battle_state;
    public Enemy_StunnedState stunned_state;


    [Header("Battle details")]
    public float battle_move_speed = 3f;
    public float attack_distance = 2f;
    public float attack_cooldown = 0.5f;
    public bool can_chase_player;


    [Space]
    public float battle_time_duration = 5f;
    public float min_retreat_distance = 1f;
    public Vector2 retreat_velocity;


    [Header("Stunned state details")]
    public float stunned_duration = 1f;
    public Vector2 stunned_velocity = new Vector2(7, 7);
    [SerializeField] protected bool can_be_stunned;


    [Header("Movement details")]
    public float idle_time = 2f;
    public float move_speed = 1.4f;
    [Range(0, 2)]
    public float move_anim_speed_multiplier = 1f;


    [Header("Player detection")]
    [SerializeField] private LayerMask what_is_player;
    [SerializeField] private Transform player_check;
    [SerializeField] private float player_check_distance = 10f;
    public Transform player { get; private set; }
    public float active_slow_multiplier { get; private set; } = 1f;


    public float GetMoveSpeed() => move_speed * active_slow_multiplier;

    public float GetBattleMoveSpeed() => battle_move_speed * active_slow_multiplier;


    protected override void Awake()
    {
        base.Awake();

        health = GetComponent<Enemy_Health>();
        stats = GetComponent<Entity_Stats>();
        combat = GetComponent<Entity_Combat>();
        vfx = GetComponent<Entity_VFX>();
    }


    public void MakeUntargetable(bool can_be_targeted)
    {
        if (can_be_targeted == false)
            gameObject.layer = LayerMask.NameToLayer("Untargetable");
        else
            gameObject.layer = LayerMask.NameToLayer("Enemy");
    }


    public virtual void SpecialAttack()
    {
        
    }


    protected override IEnumerator SlowDownEntityCo(float duration, float slow_multiplier)
    {
        active_slow_multiplier = 1 - slow_multiplier;

        anim.speed = anim.speed * active_slow_multiplier;

        yield return new WaitForSeconds(duration);
        StopSlowDown();
    }


    public override void StopSlowDown()
    {
        active_slow_multiplier = 1f;
        anim.speed = 1f;

        base.StopSlowDown();
    }


    public void EnableCounterWindow(bool enable) => can_be_stunned = enable;


    public override void EntityDeath()
    {
        base.EntityDeath();

        state_machine.ChangeState(dead_state);
    }


    private void HandlePlayerDeath()
    {
        state_machine.ChangeState(idle_state);
    }


    public void TryEnterBattleState(Transform player)
    {
        if (state_machine.current_state == battle_state || state_machine.current_state == attack_state)
            return;

        this.player = player;
        state_machine.ChangeState(battle_state);
    }
    

    public void DestroyGameObjectWithDelay(float delay = 10)
    {
        Destroy(gameObject, delay);
    }


    public Transform GetPlayerReference()
    {
        if (player == null)
            player = PlayerDetected().transform;
        return player;
    }


    public RaycastHit2D PlayerDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(player_check.position, Vector2.right * facing_dir, player_check_distance, what_is_player | what_is_ground);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return default;

        return hit;
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(player_check.position, new Vector3(player_check.position.x + (facing_dir * player_check_distance), player_check.position.y));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(player_check.position, new Vector3(player_check.position.x + (facing_dir * attack_distance), player_check.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(player_check.position, new Vector3(player_check.position.x + (facing_dir * min_retreat_distance), player_check.position.y));
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
