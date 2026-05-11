using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D col;


    public Enemy_DeadState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        col = enemy.GetComponent<Collider2D>();
    }


    public override void Enter()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        state_machine.SwitchOffStateMachine();
        enemy.DestroyGameObjectWithDelay();
    }
}
