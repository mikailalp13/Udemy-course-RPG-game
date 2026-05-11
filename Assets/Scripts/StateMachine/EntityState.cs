using UnityEngine;

public abstract class EntityState
{
    protected StateMachine state_machine;
    protected string anim_bool_name;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Entity_Stats stats;

    protected float state_timer;
    protected bool trigger_called;


    public EntityState(StateMachine state_machine, string anim_bool_name)
    {
        this.state_machine = state_machine;
        this.anim_bool_name = anim_bool_name;
    }
    

    public virtual void Enter()
    {
        anim.SetBool(anim_bool_name, true);
        trigger_called = false;
    }


    public virtual void Update()
    {
        state_timer -= Time.deltaTime;
        UpdateAnimationParameters();
    }


    public virtual void Exit()
    {
        anim.SetBool(anim_bool_name, false);
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
        float attack_speed = stats.offense.AttackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultiplier", attack_speed);
    }
}
