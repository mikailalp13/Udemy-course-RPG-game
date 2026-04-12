using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skill_manager;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;

        anim = player.anim;
        rb = player.rb;
        input = player.input;
        stats = player.stats;
        skill_manager = player.skill_manager;
    }

    public override void Update()
    {
        base.Update();

        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skill_manager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }

        if (input.Player.UltimateSpell.WasPressedThisFrame() && skill_manager.domain_expansion.CanUseSkill())
        {
            if (skill_manager.domain_expansion.InstantDomain())
                skill_manager.domain_expansion.CreateDomain();
            else 
                stateMachine.ChangeState(player.domainExpansionState);

            skill_manager.domain_expansion.SetSkillOnCooldown();
        }
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private bool CanDash()
    {
        if (skill_manager.dash.CanUseSkill() == false)
            return false; 

        if (player.wall_detected)
            return false;

        if (stateMachine.current_state == player.dashState || stateMachine.current_state == player.domainExpansionState)
            return false;

        return true;
    }
}
