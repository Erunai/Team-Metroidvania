using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private float horizontal;

    public float speed = 8f;
    public float jumpingPower = 16f;

    private bool isFacingRight = true;
    private bool velocityHalfed = false;
    private bool isJumping;

    [Space]

    [SerializeField] float knockBackTimer = 1f;
    [SerializeField] float knockBackForceY;
    [SerializeField] float knockBackForceX;
    private float knockBackCounter;

    [Space]

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] float maxGrav = 5f;

    private Animator animator;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>(); // -- TODO: Set up animation
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter <= 0)
        {
            horizontal = Input.GetAxisRaw("Horizontal"); // Returns the value of -1, 0 or +1, depending on the move direction input (A, D, left-arrow, right-arrow)

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
            {
                Debug.Log("Jump");
                rb.AddForce(new Vector2(rb.linearVelocityX, jumpingPower), ForceMode2D.Impulse);
                isJumping = true;
                velocityHalfed = false;
                animator.SetTrigger("Jump");
                animator.SetBool("Grounded", false);
            }
            else if (Mathf.Abs(horizontal) > Mathf.Epsilon)
            {
                animator.SetInteger("AnimState", 1);
            }
            else // Idle
            {
                // Can add tiny timer to delay animation state change
                animator.SetInteger("AnimState", 0);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }

            if (!isJumping && !isGrounded() && rb.linearVelocityY > 0 && !velocityHalfed)
            {
                rb.linearVelocityY = rb.linearVelocityY * 0.5f;
                velocityHalfed = true;
            }

            if ((!isJumping && !isGrounded() || rb.linearVelocityY < 0))
            {
                rb.gravityScale = Mathf.Clamp(rb.gravityScale * 1.008f, 3f, maxGrav);
            }
            else
            {
                rb.gravityScale = 3f;
            }

            if (isGrounded())
            {
                animator.SetBool("Grounded", true);
            }

            if (rb.linearVelocityY < 0)
            {
                animator.SetBool("Grounded", false);
            }

            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocityY);

            animator.SetFloat("AirSpeedY", rb.linearVelocityY - Mathf.Epsilon);

            flip();
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void flip() // Flip player (gameobject) scale on the x axis to flip direction
    {
        if ((isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool isGrounded()
    {
        //Debug.Log("IsGrounded Check");
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackTimer;
        animator.SetTrigger("Hurt");
        rb.linearVelocity = new Vector2(knockBackForceX * -transform.localScale.x, knockBackForceY);
    }
}
