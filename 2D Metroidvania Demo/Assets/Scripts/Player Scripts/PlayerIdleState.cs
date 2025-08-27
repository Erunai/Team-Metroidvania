using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");
        player.Animator.SetInteger("AnimState", 0);
        player.RB.gravityScale = player.NormalGrav;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGrounded())
            stateMachine.ChangeState(player.JumpState); // jump

        else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.01f)
            stateMachine.ChangeState(player.WalkState); // move

        else if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.AttackState); // attack

        else if (!player.IsGrounded())
            stateMachine.ChangeState(player.FallState); // fall
    }
}
