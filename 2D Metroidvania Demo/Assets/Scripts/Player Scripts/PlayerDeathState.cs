using System.Collections;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(PlayerController player, PlayerStateManager sm) : base(player, sm) { }


    // This is now handled in Player Health Controller
    public override void Enter()
    {
        Debug.Log("Player has died.");
        // Right now this does nothing other than disable the player controls - there is no death animation set up
        player.Animator.SetTrigger("Death");
        player.RB.linearVelocity = Vector2.zero;
        player.StartCoroutine(player.DeathState.RespawnCoroutine());
        player.enabled = false; // disables PlayerController script
    }

    public IEnumerator RespawnCoroutine()
    {
        Debug.Log("Respawn Coroutine Started");
        // Placeholder for respawn logic
        yield return new WaitForSeconds(2f); // wait for 2 seconds before respawning
        // Implement respawn logic here (e.g., reset position, health, etc.)
    }
}
