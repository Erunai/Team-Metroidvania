using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int maxHealth = 2;
    private int currentHealth;

    [Header("KnockBack")]
    [SerializeField] bool knockBackEnabled = false;
    [SerializeField] float knockBackTimer = 1f;
    private float knockBackCounter;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockBackCounter = knockBackTimer;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter > 0)
        {
            knockBackCounter -= Time.deltaTime;
            return; // Skip the rest of the update if knockback is active
        }
    }

    public void DamageEnemy()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            Destroy(gameObject); // Destroy the enemy object -- Placeholder until we add an animation with the Destroy f(x) in-built
        }
        if (knockBackEnabled) KnockBack(1.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthController.instance.DamagePlayer(); // Call the DamagePlayer method from PlayerHealthController
            PlayerController.instance.StateMachine.ChangeState(PlayerController.instance.KnockBackState); // Change the player's state to KnockBack
            // former PlayerController.instance.KnockBack(); // Call the KnockBack method from PlayerController
        }
    }

    public void KnockBack(float forceMultiplier)
    {
        knockBackCounter = knockBackTimer;
        /*
         * TODO:
         *      Impulse the enemy a tiny bit up and backwards relative to the player.
         *      
         */

        
        return;
    }
}
