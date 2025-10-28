using UnityEngine;

public class CollectibleObject : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.instance.AddScore(1); // Add 1 to the players score
            Destroy(gameObject); // Destroy the collectible object
        }
    }
}
