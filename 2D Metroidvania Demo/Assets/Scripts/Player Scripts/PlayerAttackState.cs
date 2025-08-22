using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int attackCounter;
    private float attackCooldownCounter;
    private float comboTimerCounter;

    public PlayerAttackState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        if (comboTimerCounter <= 0)
            attackCounter = 0;

        if (attackCounter % 3 == 0)
            player.Animator.SetTrigger("Attack1");
        else if (attackCounter % 3 == 1 && comboTimerCounter > 0)
            player.Animator.SetTrigger("Attack2");
        else if (attackCounter % 3 == 2 && comboTimerCounter > 0)
            player.Animator.SetTrigger("Attack3");

        attackCooldownCounter = player.attackCoolDown;
        comboTimerCounter = player.comboTimer;
        attackCounter++;
    }

    public override void LogicUpdate()
    {
        attackCooldownCounter -= Time.deltaTime;
        comboTimerCounter -= Time.deltaTime;

        if (attackCooldownCounter <= 0)
        {
            if (!player.IsGrounded())
                stateMachine.ChangeState(player.FallState);
            else
                stateMachine.ChangeState(player.IdleState);
        }
    }
}
