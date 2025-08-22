using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float wallJumpCounter;

    public PlayerWallJumpState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("PlayerWallJumpState: Entering Wall Jump State");
        wallJumpCounter = player.wallJumpTimer;

        // Push player up and away from the wall
        player.RB.AddForce(new Vector2(
            player.wallJumpForce * player.wallJumpDirection * player.wallJumpAngle.x,
            player.wallJumpForce * player.wallJumpAngle.y
        ), ForceMode2D.Impulse);

        player.Animator.SetTrigger("Jump");
    }

    public override void LogicUpdate()
    {
        wallJumpCounter -= Time.deltaTime;

        if (wallJumpCounter <= 0)
        {
            stateMachine.ChangeState(player.FallState);
            if (player.RB.linearVelocity.y > 0)
            {
                //stateMachine.ChangeState(player.JumpState);
            }
        }
    }

}
