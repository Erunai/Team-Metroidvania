using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    private float wallJumpCounter;

    public PlayerWallJumpState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        Debug.Log("Entering Wall Jump State");
        wallJumpCounter = player.WallJumpTimer;

        // Push player up and away from the wall
        player.RB.AddForce(new Vector2(
            player.WallJumpForce * player.WallJumpDirection * player.WallJumpAngle.x,
            player.WallJumpForce * player.WallJumpAngle.y
        ), ForceMode2D.Impulse);

        player.Animator.SetTrigger("Jump");
    }

    public override void LogicUpdate()
    {
        wallJumpCounter -= Time.deltaTime;

        if (wallJumpCounter <= 0)
        {
            stateMachine.ChangeState(player.FallState);
        }
    }

}
