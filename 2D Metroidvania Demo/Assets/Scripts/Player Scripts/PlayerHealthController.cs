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

                // Fix the Z axis of the death animation to be either -1 or 1
                // depending on the direction the player is facing
                float zRotation = transform.rotation.eulerAngles.z;
                if (zRotation > 0)
                {
                    zRotation = -1;
                }
                else
                {
                    zRotation = 1;
                }
                GameObject deathAnim = Instantiate(deathAnimation, transform.position, Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, zRotation)));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hazard")
        {
            DamagePlayer();
        }
    }

    public void SetHealth(int health)
    {
        currentHealth = health;
        healthBar.SetHealth(currentHealth);
    }
}
