using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealthController.instance.SetHealth(0); // Also updates UI
            LevelManager.instance.RespawnPlayer();
        }
    }
}
