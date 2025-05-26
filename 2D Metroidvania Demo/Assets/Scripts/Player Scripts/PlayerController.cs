using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // TODO: Refactor code
    
    public static PlayerController instance;

    private float horizontal;

    [Header("Movement Speed")]
    public float speed = 8f;
    [SerializeField] float airMoveSpeed = 12f;
    public float jumpingPower = 16f;

    private bool isFacingRight = true;
    private bool velocityHalfed = false;
    private bool isJumping;

    [Space]
    [Header("Attacking")]
    // [SerializeField] float attackDamage = 1f; // maybe add this later
    // [SerializeField] float attackSpeed = 1f; // maybe add this later
    private float attackCounter = 0f;
    [SerializeField] float attackCoolDown = 0.5f;
    private float attackCoolDownCounter;
    [SerializeField] float comboTimer = 1.0f; // Time between attacks for combo animation
    private float comboTimerCounter;
    //private bool isAttacking; // maybe ? -- Maybe use to see if a player can dash

    [Space]
    [Header("Dash")]
    [SerializeField] float dashingPower = 20f;
    [SerializeField] float dashingTime = 1f;
    [SerializeField] float dashingCooldown = 0.5f;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public bool canDash;

    [Space]
    [Header("KnockBack")]
    [SerializeField] float knockBackTimer = 1f;
    [SerializeField] float knockBackForceY;
    [SerializeField] float knockBackForceX;
    private float knockBackCounter;

    [Space]
    [Header("Wall Sliding")]
    [SerializeField] float wallSlideSpeed = 0.4f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] Vector2 wallCheckSize;
    private bool isTouchingWall;
    private bool isWallSliding;

    [Space]
    [Header("Wall Jumping")]
    [SerializeField] float wallJumpForce = 18f;
    [SerializeField] float wallJumpDirection = -1;
    [SerializeField] Vector2 wallJumpAngle;
    [SerializeField] float wallJumpTimer = 0.2f;
    private float wallJumpCounter;

    [Space]
    [Header("Misc")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] float maxGrav = 5f;
    [SerializeField] float normalGrav = 3f;


    bool wallSliding = false;
    bool wallSlidingLeft = false;

    private Animator animator;

    public bool isPaused;

    public PauseManager pauseManager;


    private bool canMove;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        canMove = true;
        animator = GetComponent<Animator>(); // -- TODO: Set up animation
        canDash = true;
        isDashing = false;
        attackCoolDownCounter = attackCoolDown;
        comboTimerCounter = comboTimer;
        wallJumpCounter = wallJumpTimer;
        attackCounter = 0;
        //isAttacking = false;
        wallJumpAngle.Normalize();
    }

    private void FixedUpdate()
    {
        if (pauseManager.isPaused || isDashing || !canMove || knockBackCounter > 0) return;

        if (!isWallSliding && wallJumpCounter < 0)
        {
            rb.linearVelocity = new Vector2(horizontal * speed, rb.linearVelocityY);
        }
        else
        {
            rb.AddForce(new Vector2(airMoveSpeed * horizontal, 0));
        }


        animator.SetFloat("AirSpeedY", rb.linearVelocityY);
        animator.SetBool("Grounded", isGrounded());

        if (!isJumping && !isGrounded() && rb.linearVelocityY < 0)
        {
            rb.gravityScale = Mathf.Clamp(rb.gravityScale * 1.008f, normalGrav, maxGrav);
        }
        else
        {
            rb.gravityScale = normalGrav;
        }
        WallSlide();
    }

    // Update is called once per frame
    void Update()
    {
        
        wallJumpCounter -= Time.deltaTime;
        knockBackCounter -= Time.deltaTime;
        if (attackCoolDownCounter > 0)
        {
            attackCoolDownCounter -= Time.deltaTime;
        }
        if (comboTimerCounter > 0)
        {
            comboTimerCounter -= Time.deltaTime;
        }

        if (isGrounded())
        {
            animator.SetBool("Falling", false);
        }
        isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0f, wallLayer); // Check if player is touching a wall
        if (!isTouchingWall)
        {
            animator.SetBool("WallSlide", false);
            animator.SetBool("Falling", true);
        }
        else
        {
            animator.SetBool("WallSlide", true);
            animator.SetBool("Falling", false);
        }


        horizontal = Input.GetAxisRaw("Horizontal"); // Returns the value of -1, 0 or +1, depending on the move direction input (A, D, left-arrow, right-arrow)
        WallJump();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded()) // Jump
        {
            Debug.Log("Player Jump");
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && attackCoolDownCounter < 0)
        {
            if (comboTimerCounter <= 0) // If player is still in combo timer, reset attackCounter
            {
                attackCounter = 0;
            }
            // isAttacking = true; -- Is this needed? Maybe I swap comboTimerCounter > 0 for a bool saying canAttack
            // Attack:
            if (attackCounter % 3 == 0)
            {
                animator.SetTrigger("Attack1");
            }
            else if ((attackCounter % 3 == 1) && comboTimerCounter > 0)
            {
                animator.SetTrigger("Attack2");
            }
            else if ((attackCounter % 3 ==  2) && comboTimerCounter > 0)
            {
                animator.SetTrigger("Attack3");
            }
            attackCoolDownCounter = attackCoolDown;
            comboTimerCounter = comboTimer;
            attackCounter++;
        }

        if (Input.GetKeyUp(KeyCode.Space)) // Check if player is still holding the jump button
        {
            isJumping = false;
        }

        if (!isJumping && !isGrounded() && rb.linearVelocityY > 0 && !velocityHalfed) // Half velocity when player releases jump button
        {
            rb.linearVelocityY = rb.linearVelocityY * 0.5f;
            velocityHalfed = true;
        }

        if (isGrounded())
        {
            animator.SetBool("Grounded", true);
        }

        if (rb.linearVelocityY < 0)
        {
            animator.SetBool("Grounded", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash) // Dash
        {
            StartCoroutine(Dash());
            return; // Exit Update() to prevent movement or reset gravity during dash
        }
       // WallJump();
        if (rb.linearVelocityY > 0 && !isJumping)
        {
            isJumping = true;
            animator.SetBool("Falling", false);
            animator.SetTrigger("Jump");
        }
        flip();

    }
    void WallSlide()
    {
        if (isTouchingWall && !isGrounded() && rb.linearVelocityY < 0)
        {
            isWallSliding = true;
            isJumping = false;
            animator.SetBool("WallSlide", true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("WallSlide", false);
        }

        if (isWallSliding)
        {
            rb.linearVelocityY = wallSlideSpeed;
        }
    }

    void WallJump()
    {
        if ((wallSliding || isTouchingWall) && Input.GetKeyDown(KeyCode.Space) && !isGrounded())
        {
            isJumping = true;
            velocityHalfed = false;
            
            animator.SetBool("Grounded", false);
            animator.SetBool("WallSlide", false);
            rb.AddForce(new Vector2(wallJumpForce * wallJumpDirection * wallJumpAngle.x, wallJumpForce * wallJumpAngle.y), ForceMode2D.Impulse);
           // animator.SetTrigger("Jump");
            wallJumpCounter = wallJumpTimer;
        }
    }


    private void flip() // Flip player (gameobject) scale on the x axis to flip direction
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            if (isWallSliding)
            {

            }
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            wallJumpDirection *= -1; // Change wall jump direction when flipping
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

    public void Walk(int multiplier)
    {
        canMove = false;
        rb.linearVelocity = new Vector2(multiplier * speed, rb.linearVelocityY);
    }

    private IEnumerator Dash()
    {
        transform.parent = null; // Remove parent object in case of a moving platform
        Debug.Log("Dash");
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

    private void OnDrawGizmosSelected()
    {
        // For wall check
        Gizmos.color = Color.red;
        Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
    }
}
