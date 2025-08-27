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

        if (horizontal < 0 && player.IsFacingRight || horizontal > 0 && !player.IsFacingRight)
            player.Flip();
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded())
        {
            Debug.Log("PlayerJumpState: Player pressed space, changing to Jump State");
            stateMachine.ChangeState(player.JumpState);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0)) // Probably unnecessary
        {
            Debug.Log("PlayerJumpState: Player pressed attack, changing to Attack State");
            stateMachine.ChangeState(player.AttackState);
        }
    }
}

