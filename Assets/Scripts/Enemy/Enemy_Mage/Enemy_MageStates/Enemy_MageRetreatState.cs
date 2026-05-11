using UnityEngine;

public class Enemy_MageRetreatState : EnemyState
{
    private Enemy_Mage enemy_mage;
    private Vector3 start_position;
    private Transform player;


    public Enemy_MageRetreatState(Enemy enemy, StateMachine state_machine, string anim_bool_name) : base(enemy, state_machine, anim_bool_name)
    {
        enemy_mage = enemy as Enemy_Mage;
    }


    public override void Enter()
    {
        base.Enter();

        if (player == null)
            player = enemy.GetPlayerReference();

        start_position = enemy.transform.position;

        rb.linearVelocity = new Vector2(enemy_mage.retreat_speed * -DirectionToPlayer(), 0);
        enemy.HandleFlip(DirectionToPlayer());

        enemy.MakeUntargetable(true);
        enemy.vfx.DoImageEchoEffect(1f);
    }


    public override void Update()
    {
        base.Update();

        bool reached_max_distance = Vector2.Distance(enemy.transform.position, start_position) > enemy_mage.retreat_max_distance;

        if (reached_max_distance || enemy_mage.CantMoveBackwards())
            state_machine.ChangeState(enemy_mage.mage_spell_cast_state);
    }


    public override void Exit()
    {
        base.Exit();

        enemy.vfx.StopImageEchoEffect();
        enemy.MakeUntargetable(false);
    }


    protected int DirectionToPlayer()
    {
        if (player == null)
            return 0;
            
        return player.position.x > enemy.transform.position.x ? 1 : -1;
    }
}
