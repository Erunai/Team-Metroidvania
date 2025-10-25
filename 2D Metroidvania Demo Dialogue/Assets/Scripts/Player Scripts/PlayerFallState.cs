using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        Debug.Log("PlayerFallState: Entering Fall State");
        // Set animator booleans
        player.Animator.SetBool("Grounded", false);
        player.Animator.SetBool("Falling", true);
    }

    public override void LogicUpdate()
    {
        if (player.IsGrounded()) // Checks every frame -- But also checked in PlayerController -- must refactor
        {   
            Debug.Log("PlayerFallState: Player is grounded");
            player.Animator.SetBool("Falling", false);
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }

    // FallState
    public override void PhysicsUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Air control
        player.RB.linearVelocity = new Vector2(horizontal * player.Speed, player.RB.linearVelocity.y);

        if (horizontal < 0 && player.IsFacingRight || horizontal > 0 && !player.IsFacingRight)
            player.Flip();

        // Gravity scaling
        if (player.RB.linearVelocity.y < 0)
            player.RB.gravityScale = Mathf.Clamp(player.RB.gravityScale * 1.1f, player.NormalGrav, player.MaxGrav);
        else
            player.RB.gravityScale = player.NormalGrav;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (Input.GetKeyDown(KeyCode.Space) && PlayerController.instance.CanDoubleJump)
        {
            player.CanDoubleJump = false;
            stateMachine.ChangeState(player.JumpState);
        }
    }
}
