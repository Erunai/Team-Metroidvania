using UnityEngine;

public class PlayerKnockBackState : PlayerState
{
    private float knockBackCounter;

    public PlayerKnockBackState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        knockBackCounter = player.knockBackTimer;
        player.Animator.SetTrigger("Hurt");

        player.RB.linearVelocity = new Vector2(
            player.knockBackForceX * -player.transform.localScale.x,
            player.knockBackForceY
        );
    }

    public override void LogicUpdate()
    {
        knockBackCounter -= Time.deltaTime;

        if (knockBackCounter <= 0)
        {
            if (player.IsGrounded())
                stateMachine.ChangeState(player.IdleState);
            else
                stateMachine.ChangeState(player.FallState);
        }
    }
}
