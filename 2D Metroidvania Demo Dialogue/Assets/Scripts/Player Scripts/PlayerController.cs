using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [Header("Movement")]
    public float Speed = 8f;
    //public float airMoveSpeed = 12f;
    public float JumpingPower = 16f;

    [Header("Attack")]
    public float AttackCoolDown = 0.5f;
    public float ComboTimer = 1f;

    [Header("Dash")]
    public float DashingPower = 20f;
    public float DashingTime = 0.2f;
    public float DashingCooldown = 0.5f;

    [Header("Knock Back")]
    public float KnockBackTimer = 0.3f;
    public float KnockBackForceX = 2.5f;
    public float KnockBackForceY = 5f;

    [Header("Wall Mechanics")]
    public float WallSlideSpeed = 0.4f;
    public LayerMask WallLayer;
    public Transform WallCheckPoint;
    public Vector2 WallCheckSize = new Vector2(0.5f, 1f);
    public float WallJumpForce = 10f;
    public float WallJumpDirection = -1;
    public Vector2 WallJumpAngle = new Vector2(1, 2);
    public float WallJumpTimer = 0.2f;

    [Header("Ground Check")]
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    public float GroundCheckRadius = 0.3f;
    public float NormalGrav = 3f;
    public float MaxGrav = 5f;

    // Components and States
    public Rigidbody2D RB { get; private set; }
    public Animator Animator { get; private set; }
    public bool IsFacingRight { get; set; } = true;
    public bool CanDash { get; set; } = true;
    public bool CanDoubleJump { get; set; } = true;

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

    // --- Dialogue/ Cutscene Freeze (ADDED) ---
    private bool _frozen;
    public bool IsFrozen => _frozen;

    /// <summary>
    /// Freeze/unfreeze the player (used by dialogue/cutscenes).
    /// </summary>
    public void SetFrozen(bool value)
    {
        _frozen = value;

        if (_frozen)
        {
            // kill motion immediately
            if (RB != null)
            {
                RB.linearVelocity = Vector2.zero; // new physics API
                RB.linearVelocity = Vector2.zero;       // classic API (harmless if unused)
            }

            // keep animator sane/idle while frozen
            if (Animator != null)
            {
                Animator.SetFloat("AirSpeedY", 0f);
                Animator.SetBool("Grounded", true);
                // If you drive other params (e.g., Speed), you can zero them too:
                // Animator.SetFloat("Speed", 0f);
            }
        }
    }

    private void Awake()
    {
        instance = this;

        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        // State Machine and States
        StateMachine = new PlayerStateManager(); // Leave as new -- we don't want this to be a singleton
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
        StateMachine.Initialize(IdleState); // Player begins in idle
        WallJumpAngle.Normalize(); // Ensure the wall jump angle is a unit vector
    }

    private void Update()
    {
        // --- EARLY OUT WHILE FROZEN (ADDED) ---
        if (_frozen)
        {
            // keep still & stable each frame in case something sets velocity
            if (RB != null)
            {
                RB.linearVelocity = Vector2.zero;
                RB.linearVelocity = Vector2.zero;
            }

            // minimal animator upkeep so you look idle/grounded
            if (Animator != null)
            {
                Animator.SetFloat("AirSpeedY", 0f);
                Animator.SetBool("Grounded", true);
            }

            return; // skip state machine while frozen
        }

        StateMachine.CurrentState.HandleInput();
        StateMachine.CurrentState.LogicUpdate();
        SetAnimatorVariables(); // TODO: Optimize by only calling when necessary
    }

    private void FixedUpdate()
    {
        // --- EARLY OUT WHILE FROZEN (ADDED) ---
        if (_frozen)
        {
            if (RB != null)
            {
                RB.linearVelocity = Vector2.zero;
                RB.linearVelocity = Vector2.zero;
            }
            return; // skip physics updates while frozen
        }

        StateMachine.CurrentState.PhysicsUpdate();
    }

    // --- Utilities ---
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, GroundLayer);
    }

    public bool IsTouchingWall()
    {
        //return Physics2D.OverlapBox(WallCheckPoint.position, WallCheckSize, 0f, WallLayer);
        return Physics2D.OverlapBox(WallCheckPoint.position, WallCheckSize, 0f, GroundLayer);
    }

    public void Flip()
    {
        // Rotate the player's sprite by scaling the x axis by -1
        IsFacingRight = !IsFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale; // Apply the new scale to the transform
        WallJumpDirection *= -1; // Change wall jump direction
    }

    // --- Animations ---
    public void SetAnimatorVariables()
    {
        Animator.SetFloat("AirSpeedY", RB.linearVelocity.y); // Y-velocity
        Animator.SetBool("Grounded", IsGrounded());
    }
}