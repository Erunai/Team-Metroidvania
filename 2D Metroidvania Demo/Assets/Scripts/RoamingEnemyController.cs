using UnityEngine;

public class RoamingEnemyController : MonoBehaviour
{
    Transform parentTransform;
    [SerializeField] GameObject roamToLeft;
    [SerializeField] GameObject roamToRight;
    [Space]
    [SerializeField] bool moveLeft;
    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] float idleTimer;

    private float leftLim;
    private float rightLim;
    private float idleCounter;
    private bool timerDone;

    private bool canMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idleCounter = idleTimer;
        leftLim = roamToLeft.transform.position.x;
        rightLim = roamToRight.transform.position.x;
        canMove = true;
        parentTransform = GetComponentInParent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        idleCounter -= Time.deltaTime;
        if (moveLeft)
        {
            parentTransform.localScale = new Vector3(1, 1, 1);

        }
        else
        {
            parentTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void FixedUpdate()
    {
        if (!moveLeft && transform.position.x >= rightLim)
        {
            canMove = false;
            idleCounter = idleTimer;
            Debug.Log("Idle Timer Reset");
            moveLeft = true;
        }
        else if (moveLeft && transform.position.x <= leftLim)
        {
            canMove = false;
            idleCounter = idleTimer;
            Debug.Log("Idle Timer Reset");
            moveLeft = false;
        }
        else
        {
            canMove = true;
        }
        

        Debug.Log(canMove.ToString());

        if (canMove && idleCounter < 0)
        {
            if (moveLeft)
            {
                transform.Translate(Vector2.left * Time.fixedDeltaTime * moveSpeed);
                
            }
            else
            {
                transform.Translate(Vector2.right * Time.fixedDeltaTime * moveSpeed);
            }
        }

    }
}
