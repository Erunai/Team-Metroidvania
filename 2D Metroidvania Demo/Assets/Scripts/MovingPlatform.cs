using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [SerializeField] float pauseTimer = 2f;
    [SerializeField] float moveSpeed = 5f;
    
    [SerializeField] bool goToPointA = true;

    private float pauseCounter = -1f;

    private Vector3 nextPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nextPos = goToPointA ? pointA.position : pointB.position; // Set direction on start -- could change to an arraylist
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseCounter >= 0)
        {
            pauseCounter -= Time.deltaTime;
        }
        else // Only move if pause counter is done
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPos, moveSpeed * Time.deltaTime); // Move towards next target
            if (transform.position == nextPos)
            {
                nextPos = (nextPos == pointA.position) ? pointB.position : pointA.position;
                pauseCounter = pauseTimer;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
