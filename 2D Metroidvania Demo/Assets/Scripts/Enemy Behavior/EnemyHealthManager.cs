using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public static EnemyHealthManager instance;
    public int maxHealth = 1; // The maximum health of the enemy
    private int _currentHealth; // The current health of the enemy

    private Animator _animator; // Reference to the Animator component

    public bool hasHurtAnimation = false; // Does the enemy have a hurt animation?

    public MonoBehaviour enemyMovementScript; // Reference to the enemy's movement script
    public float deathAnimTime = 1f; // Time to wait before destroying the enemy after death animation


    private void Awake()
    {
        instance = this;
        _currentHealth = maxHealth; // Initialize current health to maximum health
        _animator = GetComponent<Animator>();
    } 
    
    public void Hurt(int damage = 1)
    {
        _currentHealth -= damage; // Reduce current health by damage amount
        if (_currentHealth <= 0)
        {
            Die();
            return;
        }
        if (_animator != null && hasHurtAnimation)
        {
            _animator.SetTrigger("Hurt"); // Trigger hurt animation if available
        }
    }
    public void Die()
    {
        enemyMovementScript.enabled = false; // Disable enemy movement
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (boxCollider2D != null)
        {
            boxCollider2D.enabled = false; // Disable the collider to prevent further interactions
        }
        if (rb2d != null)
        {
            rb2d.linearVelocity = Vector2.zero; // Stop any movement
            rb2d.bodyType = RigidbodyType2D.Kinematic; // Make the Rigidbody kinematic to prevent physics interactions
        }
        if (_animator != null)
        {
            _animator.SetTrigger("Die");
            Destroy(gameObject, deathAnimTime); // Destroy the enemy after 1 second to allow death animation to play
        }
        else
        {
            Debug.Log("Animator is Null");
            Destroy(gameObject); // Destroy the enemy immediately if no animator is present
        }
        // Additional death logic can be added here (e.g., disable enemy, play sound, etc.)
    }
}
