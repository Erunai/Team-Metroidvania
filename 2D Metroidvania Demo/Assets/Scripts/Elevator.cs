using UnityEngine;

public class Elevator : MonoBehaviour
{
    /*
     * Behaviour:
     *      When at idle:
     *          OnTriggerEnter2d:
     *              Player is childed to elevator
     *              elevator moves after a delay
     *              after getting to a certain point it stops
     *          OnTriggerExit2d:
     *              Player is unchilded
     *              elevator moves down after a delay
     *              after getting to it's resting point, it stops
     */
    [SerializeField] Transform targetObj;
    [SerializeField] float moveSpeed;
    [SerializeField] float delayTimer;

    [SerializeField] bool goBackToStart = true;

    private Vector3 startPos;

    private float delayCounter = -1f;
    private bool charIsOn = false;

    private bool reachedPos = false;

    private Vector3 targetPos;
    private Vector3 moveToPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = targetObj.position;
        startPos = transform.position;
        moveToPos = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (delayCounter >= 0f)
        {
            delayCounter -= Time.deltaTime;
        }

        reachedPos = transform.position == moveToPos;

        if (reachedPos)
        {
            delayCounter = delayTimer;
            if (goBackToStart && !charIsOn)
            {
                moveToPos = startPos;
            }
        }

        if (!reachedPos && delayCounter < 0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (reachedPos)
        {
            delayCounter = delayTimer;
            moveToPos = moveToPos == startPos ? targetPos : startPos;
            reachedPos = false;
        }
        if (collision.gameObject.tag.Equals("Player"))
        {
            // Set collider object's parent to be this game object
            collision.gameObject.transform.parent = transform;
            charIsOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            // Set collider object's parent to be this game object
            collision.gameObject.transform.parent = null;
            charIsOn = false;
        }
    }
}
