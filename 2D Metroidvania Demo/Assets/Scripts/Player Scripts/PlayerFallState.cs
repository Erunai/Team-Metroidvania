using UnityEngine;

public class PlayerFallState : PlayerState
{
    public PlayerFallState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("PlayerFallState: Entering Fall State");
        player.Animator.SetBool("Grounded", false);
        player.Animator.SetBool("Falling", true);
    }

    public override void LogicUpdate()
    {
        if (player.IsGrounded())
        {   
            Debug.Log("PlayerFallState: Player is grounded, changing to Idle State");
            player.Animator.SetBool("Falling", false);
            stateMachine.ChangeState(player.IdleState);
        }

        if (player.IsTouchingWall())
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
    }

    // FallState.cs
    public override void PhysicsUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Air control
        player.RB.linearVelocity = new Vector2(horizontal * player.speed, player.RB.linearVelocity.y);

        if (horizontal < 0 && player.IsFacingRight || horizontal > 0 && !player.IsFacingRight)
            player.Flip();

        // Gravity scaling
        if (player.RB.linearVelocity.y < 0)
            player.RB.gravityScale = Mathf.Clamp(player.RB.gravityScale * 1.008f, player.normalGrav, player.maxGrav);
        else
            player.RB.gravityScale = player.normalGrav;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }
}
