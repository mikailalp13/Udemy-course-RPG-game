using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    private Camera main_camera;
    public Player_SwordThrowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skill_manager.sword_throw.EnableDots(true);

        if (main_camera != Camera.main)
            main_camera = Camera.main;

    }

    public override void Update()
    {
        base.Update();

        Vector2 dir_to_mouse = DirectionToMouse();

        player.SetVelocity(0, rb.linearVelocity.y);
        player.HandleFlip(dir_to_mouse.x);
        skill_manager.sword_throw.PredictTrajectory(dir_to_mouse);


        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("swordThrowPerformed", true);

            skill_manager.sword_throw.EnableDots(false);
            skill_manager.sword_throw.ConfirmTrajectory(dir_to_mouse);
        }

        if (input.Player.RangeAttack.WasReleasedThisFrame() || trigger_called)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        anim.SetBool("swordThrowPerformed", false);
        skill_manager.sword_throw.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 player_position = player.transform.position;
        Vector2 world_mouse_position = main_camera.ScreenToWorldPoint(player.mouse_position);

        Vector2 direction = world_mouse_position - player_position;

        return direction.normalized;
    }
}
