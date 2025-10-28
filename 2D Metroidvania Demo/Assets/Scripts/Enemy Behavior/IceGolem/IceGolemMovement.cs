using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] Transform limitLeft;
    [SerializeField] Transform limitRight;
    [SerializeField] float moveSpeed = 2f;
    private Animator _animator;
    [SerializeField] float idleDuration = 2f;
    private float _idleTimer;
    private bool _isIdle;
    private Rigidbody2D _rigidbody2D;
    private bool _moveLeft;
    private float _xScale;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _idleTimer = idleDuration;
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _isIdle = true; // Start in idle state
        _moveLeft = true; // Start by moving left

        _xScale = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement logic
        moveGolem();
    }

    void moveGolem()
    {
        // If idle, count down the timer
        if (_isIdle)
        {
            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0f)
            {
                _isIdle = false; // Exit idle state
                _animator.SetBool("isMoving", true); // Set moving animation
            }
            else
            {
                _rigidbody2D.linearVelocity = Vector2.zero; // Stay still while idle
                return;
            }
        }
        // Move the golem left or right between limits
        if (_moveLeft)
        {
            transform.localScale = new Vector3(-Mathf.Abs(_xScale), transform.localScale.y, transform.localScale.z);
            _rigidbody2D.linearVelocity = new Vector2(-moveSpeed, _rigidbody2D.linearVelocity.y);
            if (transform.position.x <= limitLeft.position.x)
            {
                _moveLeft = false; // Change direction to right
                // Enter idle state
                enterIdleState();
            }
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Abs(_xScale), transform.localScale.y, transform.localScale.z);
            _rigidbody2D.linearVelocity = new Vector2(moveSpeed, _rigidbody2D.linearVelocity.y);
            if (transform.position.x >= limitRight.position.x)
            {
                _moveLeft = true; // Change direction to left
                enterIdleState();
            }
        }
    }

    void enterIdleState() 
    {
        _isIdle = true;
        _idleTimer = idleDuration;
        _animator.SetBool("isMoving", false); // Set idle animation
        _rigidbody2D.linearVelocity = Vector2.zero; // Stop movement
    }
}
