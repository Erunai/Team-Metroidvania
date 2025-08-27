using UnityEngine;

public class PlayerAttackState : PlayerState
{
    private int attackCounter;
    private float attackCooldownCounter;
    private float comboTimerCounter;

    public PlayerAttackState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        int attackNumber = attackCounter % 3;
        Debug.Log("Attack State: Attack Number" + attackNumber);
        if (comboTimerCounter <= 0)
            attackCounter = 0;

        if (attackNumber == 0)
            player.Animator.SetTrigger("Attack1");
        else if (attackNumber == 1 && comboTimerCounter > 0)
            player.Animator.SetTrigger("Attack2");
        else if (attackNumber == 2 && comboTimerCounter > 0)
            player.Animator.SetTrigger("Attack3");

        attackCooldownCounter = player.AttackCoolDown;
        comboTimerCounter = player.ComboTimer;
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
