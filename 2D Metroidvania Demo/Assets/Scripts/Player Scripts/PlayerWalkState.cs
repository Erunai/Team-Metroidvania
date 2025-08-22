using UnityEngine;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        base.Enter();
        player.Animator.SetInteger("AnimState", 1);
        player.RB.gravityScale = player.normalGrav;
    }

    public override void LogicUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        player.RB.linearVelocity = new Vector2(horizontal * player.speed, player.RB.linearVelocity.y);

        if (horizontal < 0 && player.IsFacingRight || horizontal > 0 && !player.IsFacingRight)
            player.Flip();

        if (horizontal == 0)
            stateMachine.ChangeState(player.IdleState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded())
            stateMachine.ChangeState(player.JumpState);

        if (!player.IsGrounded())
            stateMachine.ChangeState(player.FallState);
        
    }
    public override void HandleInput()
    {   
        base.HandleInput();
    }
}
