using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private float horizontal;

    [Header("Movement Speed")]
    public float speed = 8f;
    public float jumpingPower = 16f;

    private bool isFacingRight = true;
    private bool velocityHalfed = false;
    private bool isJumping;

    [Space]
    [Header("Dash")]
    [SerializeField] float dashingPower = 20f;
    [SerializeField] float dashingTime = 1f;
    [SerializeField] float dashingCooldown = 0.5f;
    private bool isDashing;
    private bool canDash;

    [Space]
    [Header("KnockBack")]
    [SerializeField] float knockBackTimer = 1f;
    [SerializeField] float knockBackForceY;
    [SerializeField] float knockBackForceX;
    private float knockBackCounter;

    [Space]
    [Header("Misc")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] float maxGrav = 5f;
    [SerializeField] float normalGrav = 3f;

    private Animator animator;

    public bool isPaused;

    public PauseManager pauseManager;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        animator = GetComponent<Animator>(); // -- TODO: Set up animation
        canDash = true;
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseManager.isPaused || isDashing)
        {
            return;
        }
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

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
            {
                StartCoroutine(Dash());
                return;
            }

            if (!isJumping && !isGrounded() && rb.linearVelocityY > 0 && !velocityHalfed)
            {
                rb.linearVelocityY = rb.linearVelocityY * 0.5f;
                velocityHalfed = true;
            }

            if ((!isJumping && !isGrounded() || rb.linearVelocityY < 0))
            {
                rb.gravityScale = Mathf.Clamp(rb.gravityScale * 1.008f, normalGrav, maxGrav);
            }
            else
            {
                rb.gravityScale = normalGrav;
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

    public void PlayerDeath()
    {
        animator.SetTrigger("Death");
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float currentGrav = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = currentGrav;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
