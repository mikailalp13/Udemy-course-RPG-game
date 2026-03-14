using UnityEngine;

public class Player_DomainExpansionState : PlayerState
{
    private Vector2 original_position;
    private float original_gravity;
    private float max_distance_to_go_up;

    private bool is_levitating;
    private bool created_domain;


    public Player_DomainExpansionState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        original_position = player.transform.position;
        original_gravity = rb.gravityScale;
        max_distance_to_go_up = GetAvaliableRiseDistance();

        player.SetVelocity(0, player.rise_speed);
        player.health.SetCanTakeDamage(false);
    }

    public override void Update()
    {
        base.Update();

        if (Vector2.Distance(original_position, player.transform.position) >= max_distance_to_go_up && is_levitating == false)
            Levitate();

        if (is_levitating)
        {
            skill_manager.domain_expansion.DoSpellCasting();

            if (state_timer < 0){ 
                is_levitating = false;
                rb.gravityScale = original_gravity;
                stateMachine.ChangeState(player.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        created_domain = false;
        player.health.SetCanTakeDamage(true);
    }

    private void Levitate()
    {
        is_levitating = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0;

        state_timer = skill_manager.domain_expansion.GetDomainDuration();

        if (created_domain == false)
        {
            created_domain = true;
            skill_manager.domain_expansion.CreateDomain();
        }
    }

    private float GetAvaliableRiseDistance()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(player.transform.position, Vector2.up, player.rise_max_distance, player.what_is_ground);

        return hit.collider != null ? hit.distance -1 : player.rise_max_distance;
    }
}
