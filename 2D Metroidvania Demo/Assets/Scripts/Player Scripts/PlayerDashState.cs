using UnityEngine;
using System.Collections;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        Debug.Log("PlayerDashState: Entering Dash State");
        base.Enter();
        player.StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        player.CanDash = false;
        float originalGrav = player.RB.gravityScale;

        player.RB.gravityScale = 0f;
        player.RB.linearVelocity = new Vector2(player.transform.localScale.x * player.dashingPower, 0f);

        yield return new WaitForSeconds(player.dashingTime);

        player.RB.gravityScale = originalGrav;

        // Return to grounded or falling state
        if (player.IsGrounded())
            stateMachine.ChangeState(player.IdleState);
        else
            stateMachine.ChangeState(player.FallState);

        yield return new WaitForSeconds(player.dashingCooldown);
        player.CanDash = true;
    }
    public override void HandleInput()
    {
        // empty override to prevent state change during dash
    }
}
