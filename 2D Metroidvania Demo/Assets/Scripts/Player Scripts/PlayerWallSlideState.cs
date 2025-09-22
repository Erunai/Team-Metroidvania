using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    /*
     * TODO:
     * I want to check the Y velocity of the player when they enter this state
     * Then, if they are falling quickly, I would like them to ease into the wall slide speed
     * 
     * 22/09/2025:
     *  I am going to try changing the collision check to the ground layer to see if I need to create a seperate wall tilemap - LB
     */
    public PlayerWallSlideState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    private float _wallSlideBoost = 5f; // Speed boost while wall sliding for players when holding down
    private bool _isPressingDown = false; // Track if the player is pressing down

    public override void Enter()
    {
        Debug.Log("Entering Wall Slide State");
        player.Animator.SetBool("WallSlide", true);
        player.RB.gravityScale = player.NormalGrav;
    }

    public override void Exit()
    {
        player.Animator.SetBool("WallSlide", false);
    }

    public override void LogicUpdate()
    {
        if (!player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.FallState);
        }

        if (player.IsGrounded())
        {
           stateMachine.ChangeState(player.IdleState);
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
        if (player.WallJumpDirection > 0 && horizontal > 0)
        {
            player.RB.linearVelocity = new Vector2(player.Speed * player.WallJumpDirection, player.RB.linearVelocity.y);
            stateMachine.ChangeState(player.FallState);
        }

        // Player is on the RIGHT wall
        if (player.WallJumpDirection < 0 && horizontal < 0)
        {
            player.RB.linearVelocity = new Vector2(player.Speed * player.WallJumpDirection, player.RB.linearVelocity.y);
            stateMachine.ChangeState(player.FallState);
        }

        // If the player is pressing down, they should fall faster
        _isPressingDown = Input.GetAxisRaw("Vertical") < 0;

    }

    public override void PhysicsUpdate()
    {
        float targetYVelocity = -player.WallSlideSpeed;

        if (_isPressingDown) { 
            targetYVelocity -= _wallSlideBoost; // Apply boost if pressing down
        }
        player.RB.linearVelocity = new Vector2(player.RB.linearVelocity.x, targetYVelocity);
    }
}
