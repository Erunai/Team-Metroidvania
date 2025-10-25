using UnityEngine;

public class PlayerKnockBackState : PlayerState
{
    private float _knockBackCounter;

    public PlayerKnockBackState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        Debug.Log("Entering KnockBack State");
        // Set knockback timer
        _knockBackCounter = player.KnockBackTimer;
        player.Animator.SetTrigger("Hurt");

        // Did we want to knock the player away from the thing that hit them? If so, we need to set a bool up in the player controller for knockBack direction
        // Knock back player physically
        player.RB.linearVelocity = new Vector2(
            player.KnockBackForceX * -player.transform.localScale.x,
            player.KnockBackForceY
        );
    }

    public override void LogicUpdate()
    {
        // Knockback timer countdown + state change
        _knockBackCounter -= Time.deltaTime;
        if (_knockBackCounter <= 0)
        {
            if (player.IsGrounded())
            {
                player.RB.linearVelocity = new Vector2(0, player.RB.linearVelocity.y); // Cancel the horizontal velocity out before changing state
                stateMachine.ChangeState(player.IdleState);
            }
            else
                stateMachine.ChangeState(player.FallState);
        }
    }
}
