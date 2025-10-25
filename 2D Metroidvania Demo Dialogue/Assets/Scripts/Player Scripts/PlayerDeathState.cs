using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }

    public override void Enter()
    {
        player.Animator.SetTrigger("Death");
        player.RB.linearVelocity = Vector2.zero;
        player.enabled = false; // disables PlayerController script
    }
}
