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
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.FallState);
        }
    }
}
