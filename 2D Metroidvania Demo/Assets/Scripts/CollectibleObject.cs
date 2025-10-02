using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelManager.instance.AddScore(1); // Add 1 to the players score
            Destroy(gameObject); // Destroy the collectible object
        }
    }
}
