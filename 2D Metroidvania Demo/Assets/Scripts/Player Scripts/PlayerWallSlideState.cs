using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    private float wallSlideBoost = 5f; // Speed boost while wall sliding for players when holding down
    private bool isPressingDown = false; // Track if the player is pressing down
    public override void Enter()
    {
        player.Animator.SetBool("WallSlide", true);
        player.RB.gravityScale = player.normalGrav;
    }

    public override void Exit()
    {
        player.Animator.SetBool("WallSlide", false);
    }

    public override void LogicUpdate()
    {
        if (!player.IsTouchingWall() || player.IsGrounded())
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.WallJumpState);
        }
    }

    public override void HandleInput()
    {
        // Cannot dash while wall sliding
        // Should be able to use A or D to move left or right off of the wall
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Player is on the LEFT wall
        if (player.IsTouchingWallLeft() && horizontal > 0)
        {
            player.RB.linearVelocity = new Vector2(player.speed, player.RB.linearVelocity.y);
            stateMachine.ChangeState(player.FallState);
        }

        // Player is on the RIGHT wall
        if (!player.IsTouchingWallLeft() && horizontal < 0)
        {
            player.RB.linearVelocity = new Vector2(player.speed, player.RB.linearVelocity.y);
            stateMachine.ChangeState(player.FallState);
        }

        // If the player is pressing down, they should fall faster
        isPressingDown = Input.GetAxisRaw("Vertical") < 0;

    }

    public override void PhysicsUpdate()
    {
        float targetYVelocity = -player.wallSlideSpeed;

        if (isPressingDown) { 
            targetYVelocity -= wallSlideBoost; // Apply boost if pressing down
        }
        player.RB.linearVelocity = new Vector2(player.RB.linearVelocity.x, targetYVelocity);
    }
}
