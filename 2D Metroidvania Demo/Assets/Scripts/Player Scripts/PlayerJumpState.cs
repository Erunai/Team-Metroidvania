using System;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    private bool _increaseGrav;
    private float _horizontal;
    public override void Enter()
    {
        Debug.Log("PlayerJumpState: Entering Jump State");
        player.RB.AddForce(Vector2.up * player.JumpingPower, ForceMode2D.Impulse);
        player.Animator.SetTrigger("Jump");
        _increaseGrav = false;
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
        // TODO maybe -- allow for attack input while jumping
    }
}

