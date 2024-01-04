using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 15f;
    [Tooltip("while player is in the air, the horizontal moveSpeed is multiplied by airSpeedModifier")]
    public float airSpeedModifier = 0.8f;
    

    [Header("Dash Info")]
    public float dashSpeed = 30f;
    public float dashDuration = 0.5f;
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashDir { get; private set; } //making is public 
    //get but private set allows it to be public, but NOT VISIBLE in inspector(?)

    [Header("Wallslide/Walljump Info")]
    public float wallslideSpeed = 6f;
    [Tooltip("When a player does a wall jump, they will continue to travel horizontally for this many seconds before their xVelocity drops to 0.")]
    public float walljumpMaxDuration = 3f;
    [Tooltip("When a player does a wall jump, they will have to wait this many seconds before they can change their horizontal direction.")]
    public float wallJumpMinDuration = 0.1f;


    [Header("CollisionInfo")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckDistance;
    [SerializeField] Transform wallCheck;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;
    public int facingDir { get; private set; } = 1;
    bool facingRight = true;

    


    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion


    #region States
    public PlayerStateMachine stateMachine {  get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallslideState wallslideState { get; private set; }
    public PlayerWalljumpState walljumpState { get; private set; }

    #endregion


    private void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump"); //don't write "Air"
        //as a separate animBoolName, because both "Jump" and "Air" are
        //synonymous in our case
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallslideState = new PlayerWallslideState(this, stateMachine, "Wallslide");
        walljumpState = new PlayerWalljumpState(this, stateMachine, "Jump");
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        CheckForDashInput();
        stateMachine.currentState.Update();
        
        
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return; //you cannot dash if you're standing point blank against a wall

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);

        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity); //now we can Flip the sprite in multiple states!
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, 
            new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, 
        Vector3.down, groundCheckDistance, whatIsGround);
        
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector3.right * facingDir, 
            wallCheckDistance, whatIsGround);


    public void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x < 0 && facingRight)
            Flip();

        else if (_x > 0 && !facingRight)
            Flip();
    }







}
