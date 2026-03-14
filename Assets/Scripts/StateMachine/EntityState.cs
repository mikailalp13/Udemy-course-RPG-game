using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Entity_Stats stats;

    protected float state_timer;
    protected bool trigger_called;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        trigger_called = false;
    }
    public virtual void Update()
    {
        state_timer -= Time.deltaTime;
        UpdateAnimationParameters();
    }
    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }
    public void AnimationTrigger()
    {
        trigger_called = true;
    }

    public virtual void UpdateAnimationParameters()
    {
        
    }

    public void SyncAttackSpeed()
    {
        float attack_speed = stats.offense.attack_speed.GetValue();
        anim.SetFloat("attackSpeedMultiplier", attack_speed);
    }

}
