using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    /*
     * NOTES FOR CHANGING CHARACTER
     * The new character has the following animations:
     * Idle, run, flyUp, flyDown, die, punch, go-to-sit, sit, sit-to-go, die
     * Maybe can use the punch animation (partial) for wall slide.
     */
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
    public PlayerAttackState AttackState { get; private set; } // no longer a state
    public PlayerDashState DashState { get; private set; }
    public PlayerWallSlideState WallSlideState { get; private set; }
    public PlayerWallJumpState WallJumpState { get; private set; }
    public PlayerKnockBackState KnockBackState { get; private set; } // no longer a state
    public PlayerDeathState DeathState { get; private set; } // Was never actually a state

    private void Awake()
    {
        instance = this;

        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        // State Machine and States -- ensure they are not singletons
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
        StateMachine.Initialize(IdleState); // Player begins in idle
        WallJumpAngle.Normalize(); // Ensure the wall jump angle is a unit vector
    }

    private void Update()
    {
        StateMachine.CurrentState.HandleInput();
        StateMachine.CurrentState.LogicUpdate();
        SetAnimatorVariables(); // TODO: Optimize by only calling when necessary
    }

    private void FixedUpdate()
    {
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
        Animator.SetFloat("AirSpeedY", RB.linearVelocity.y); // Y-velocity -- Does this need to be set in idle or walking?
        Animator.SetBool("Grounded", IsGrounded()); // Does this need to be set in idle, walking, sliding, or dashing?
    }
}
