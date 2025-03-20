using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public HealthBar healthBar;

    public float invincibleTimer = 3f;
    private float invincibleCounter;
    public float alphaValue = 0.5f;

    public int currentHealth;
    public int maxHealth = 3;

    private SpriteRenderer sr;

    public GameObject deathAnimation;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        //HealthBar.instance.SetMaxHealth(currentHealth);
        healthBar.SetMaxHealth(currentHealth);
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
            if (invincibleCounter <= 0)
            {
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
            }
        }
    }

    public void DamagePlayer()
    {
        if (invincibleCounter <= 0)
        {
            currentHealth--;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0;

                GameObject deathAnim = Instantiate(deathAnimation, transform.position, transform.rotation);
                deathAnim.transform.localScale = transform.localScale;

                LevelManager.instance.RespawnPlayer();
            }
            else
            {
                PlayerController.instance.KnockBack();
                invincibleCounter = invincibleTimer;
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alphaValue);
            }
        }
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
