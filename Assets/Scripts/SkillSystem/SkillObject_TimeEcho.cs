using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wisp_move_speed = 15f;
    [SerializeField] private GameObject on_death_vfx;
    [SerializeField] private LayerMask whatIsGround;
    private bool should_move_to_player;


    private Transform player_transform;
    private Skill_TimeEcho echo_manager;
    private TrailRenderer wisp_trail;
    private Entity_Health player_health;
    private SkillObject_Health echo_health;
    private Player_SkillManager skill_manager;
    private Entity_StatusHandler status_handler;


    public int max_attacks { get; private set; }


    public void SetupEcho(Skill_TimeEcho echo_manager)
    {
        this.echo_manager = echo_manager;
        player_stats = echo_manager.player.stats;
        damage_scale_data = echo_manager.damage_scale_data;
        max_attacks = echo_manager.GetMaxAttacks();
        player_transform = echo_manager.transform.root;
        player_health = echo_manager.player.health;
        skill_manager = echo_manager.skill_manager;
        status_handler = echo_manager.player.status_handler;

        echo_health = GetComponent<SkillObject_Health>();
        wisp_trail = GetComponentInChildren<TrailRenderer>();
        wisp_trail.gameObject.SetActive(false);

        FlipToTarget();
        anim.SetBool("canAttack", max_attacks > 0);
        Invoke(nameof(HandleDeath), echo_manager.GetEchoDuration());
    }

    private void Update()
    {
        if (should_move_to_player)
            HandleWispMovement();

        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }
    }

    private void HandlePlayerTouch()
    {
        float heal_amount = echo_health.last_damage_taken * echo_manager.GetPercentOfDamageHealed();
        player_health.IncreaseHealth(heal_amount);

        float amount_in_seconds = echo_manager.GetCooldownReduceInSeconds();
        skill_manager.ReduceAllSkillCooldownBy(amount_in_seconds);

        if (echo_manager.CanRemoveNegativeEffects())
            status_handler.RemoveAllNegativeEffects();
    }

    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position, player_transform.position, wisp_move_speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player_transform.position) < 0.5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void FlipToTarget()
    {
        Transform target = FindClosestTarget();

        if (target != null && target.position.x < transform.position.x)
            transform.Rotate(0, 180, 0);
    }

    public void PerformAttack()
    {
        DamageEnemiesInRadius(target_check, 1);

        if (target_got_hit == false)
            return;

        bool can_duplicate = Random.value < echo_manager.GetDuplicateChance();
        float x_offset = transform.position.x < last_target.position.x ? 1 : -1;

        if (can_duplicate)
            echo_manager.CreateTimeEcho(last_target.position + new Vector3 (x_offset, 0));
    }

    public void HandleDeath()
    {
        Instantiate(on_death_vfx, transform.position, Quaternion.identity);

        if (echo_manager.ShouldBeWisp())
            TurnIntoWisp();
        else
            Destroy(gameObject);
    }

    private void TurnIntoWisp()
    {
        should_move_to_player = true;
        anim.gameObject.SetActive(false);
        wisp_trail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }
}
