using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifeTime = 5f;

    private bool moveLeft;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveLeft = transform.rotation.z != 90f;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }
    void FixedUpdate()
    {
        if (moveLeft)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.up * -speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Bullet hit: " + collision.tag);
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Bullet Hit Player");
            PlayerHealthController.instance.DamagePlayer();
            
        }
        //Debug.Log("Bullet Destroyed");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // For when player dashes into a bullet from behind
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Bullet Hit Player");
            PlayerHealthController.instance.DamagePlayer();
        }
        Destroy(gameObject);
    }

}
