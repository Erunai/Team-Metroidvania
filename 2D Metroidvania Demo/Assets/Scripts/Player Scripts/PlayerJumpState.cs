using System;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    private bool _increaseGrav;
    private float _horizontal;
    public override void Enter()
    {
        Debug.Log("Entering Jump State");
        Jump();
        player.RB.gravityScale = player.NormalGrav; // Reset gravity scale when entering jump
    }
    public override void LogicUpdate()
    {
        if (player.RB.linearVelocity.y < 0)
        {
            Debug.Log("PlayerJumpState: Changing to Fall State");
            stateMachine.ChangeState(player.FallState);
        }
    }

    public override void PhysicsUpdate()
    {

        // Air control
        player.RB.linearVelocity = new Vector2(_horizontal * player.Speed, player.RB.linearVelocity.y);

        // Gravity scaling
        if (_increaseGrav)
        {
            player.RB.gravityScale = Mathf.Clamp(player.RB.gravityScale * 1.1f, player.NormalGrav, player.MaxGrav);
        }

        // Flip player sprite -- should this be in PlayerController instead? --- probably
        if (_horizontal < 0 && player.IsFacingRight || _horizontal > 0 && !player.IsFacingRight)
            player.Flip();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        _horizontal = Input.GetAxisRaw("Horizontal");
        if (!Input.GetKey(KeyCode.Space) && !_increaseGrav)
        {
            _increaseGrav = true;
            Debug.Log("PlayerJumpState: Player released jump key, increasing gravity");
        }
        if (Input.GetKeyDown(KeyCode.Space) && PlayerController.instance.CanDoubleJump)
        {
            Jump();
            player.CanDoubleJump = false;
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting jump state: Reset Anim Trigger Jump");
        player.Animator.ResetTrigger("Jump"); // This isn't working ???
    }

    private void Jump()
    {
        // Reset vertical velocity before jumping to ensure consistent jump height
        player.RB.linearVelocity = new Vector2(player.RB.linearVelocity.x, 0f);
        player.RB.AddForce(Vector2.up * player.JumpingPower, ForceMode2D.Impulse);
        player.Animator.SetTrigger("Jump");
        _increaseGrav = false;
    }
}

