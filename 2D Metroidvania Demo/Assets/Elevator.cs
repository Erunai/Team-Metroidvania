using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float pauseTimer = 2f;
    [SerializeField] float yMax = 0f;
    [SerializeField] float yMin = 1f;
    [SerializeField] bool moveUp = true;

    private float pauseCount = 0;

    private void Update()
    {
        if (pauseCount >= 0)
        {
            pauseCount -= Time.deltaTime;
        }
    }

    public void FixedUpdate()
    {
        if (moveUp && pauseCount < 0)
        {
            transform.Translate(Vector2.up * moveSpeed * Time.fixedDeltaTime);
        }
        else if (!moveUp && pauseCount < 0)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.fixedDeltaTime);
        }

        if (transform.position.y >= yMax && moveUp)
        {
            moveUp = false;
            pauseCount = pauseTimer;
        }
        if (transform.position.y <= yMin && !moveUp)
        {
            moveUp = true;
            pauseCount = pauseTimer;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.transform.parent = transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.transform.parent = null;
        }
    }

}
