using System;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        Debug.Log("PlayerJumpState: Entering Jump State");
        player.RB.AddForce(Vector2.up * player.JumpingPower, ForceMode2D.Impulse);
        player.Animator.SetTrigger("Jump");
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
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Air control
        player.RB.linearVelocity = new Vector2(horizontal * player.Speed, player.RB.linearVelocity.y);

        // Gravity scaling
        if (!Input.GetKey(KeyCode.Space))
        {
            Debug.Log("PlayerJumpState: Player released jump key, increasing gravity");
            player.RB.gravityScale = Mathf.Clamp(player.RB.gravityScale * 1.1f, player.NormalGrav, player.MaxGrav);
        }
        // Flip player sprite -- should this be in PlayerController instead? --- probably
        if (horizontal < 0 && player.IsFacingRight || horizontal > 0 && !player.IsFacingRight)
            player.Flip();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        // TODO maybe -- allow for attack input while jumping
    }
}

