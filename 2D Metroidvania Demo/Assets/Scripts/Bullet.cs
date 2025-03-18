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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet Hit!");
        // Send message to damage player if collision.tag == "Player"

        Destroy(gameObject);
    }
}
