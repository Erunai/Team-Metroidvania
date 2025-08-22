using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Movement")]
    public float speed = 8f;
    //public float airMoveSpeed = 12f;
    public float jumpingPower = 16f;

    [Header("Attack")]
    public float attackCoolDown = 0.5f;
    public float comboTimer = 1f;

    [Header("Dash")]
    public float dashingPower = 20f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 0.5f;

    [Header("KnockBack")]
    public float knockBackTimer = 1f;
    public float knockBackForceX = 5f;
    public float knockBackForceY = 10f;

    [Header("Wall Mechanics")]
    public float wallSlideSpeed = 0.4f;
    public LayerMask wallLayer;
    public Transform wallCheckPoint;
    public Vector2 wallCheckSize = new Vector2(0.5f, 1f);
    public float wallJumpForce = 18f;
    public float wallJumpDirection = -1;
    public Vector2 wallJumpAngle = new Vector2(1, 2);
    public float wallJumpTimer = 0.2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.3f;
    public float normalGrav = 3f;
    public float maxGrav = 5f;

    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public bool IsFacingRight { get; set; } = true;
    public bool CanDash { get; set; } = true;

    public PlayerStateManager StateMachine { get; private set; }

    // States
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerFallState FallState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerKnockBackState KnockBackState { get; private set; }
    public PlayerDeathState DeathState { get; private set; }

    private void Awake()
    {
        instance = this;
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        StateMachine = new PlayerStateManager();

        IdleState = new PlayerIdleState(this, StateMachine);
        WalkState = new PlayerWalkState(this, StateMachine);
        JumpState = new PlayerJumpState(this, StateMachine);
        FallState = new PlayerFallState(this, StateMachine);
        AttackState = new PlayerAttackState(this, StateMachine);
        DashState = new PlayerDashState(this, StateMachine);
        WallSlideState = new PlayerWallSlideState(this, StateMachine);
        WallJumpState = new PlayerWallJumpState(this, StateMachine);
        KnockBackState = new PlayerKnockBackState(this, StateMachine);
        DeathState = new PlayerDeathState(this, StateMachine);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        wallJumpAngle.Normalize();
    }

    private void Update()
    {
        StateMachine.CurrentState.HandleInput();
        StateMachine.CurrentState.LogicUpdate();
        SetAnimatorVariables();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    // --- Utilities ---
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    public bool IsTouchingWall()
    {
        return Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0f, wallLayer);
    }

    public bool IsTouchingWallLeft()
    {
        return Physics2D.OverlapBox(new Vector2(wallCheckPoint.position.x - wallCheckSize.x / 2, wallCheckPoint.position.y), wallCheckSize, 0f, wallLayer);
    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        wallJumpDirection *= -1;
    }

    // --- Animator ---
    public void SetAnimatorVariables()
    {
        Animator.SetFloat("AirSpeedY", RB.linearVelocity.y);
        Animator.SetBool("Grounded", IsGrounded());

    }
}
