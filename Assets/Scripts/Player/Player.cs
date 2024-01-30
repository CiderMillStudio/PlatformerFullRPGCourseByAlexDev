using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : Entity
{
/*This class inherits from Entity.cs, and contains only information
 that is SPECIFICALLY used by the PLAYER and no other character.
(yaaaay polymorphism!!) */

    #region Move Info
    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 15f;
    [Tooltip("while player is in the air, the horizontal moveSpeed is multiplied by airSpeedModifier")]
    public float airSpeedModifier = 0.8f;
    public float swordReturnImpact = 9f;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
   
    #endregion 

    #region Dash Info
    [Header("Dash Info")]
    public float dashSpeed = 30f;
    public float dashDuration = 0.5f;
    // [SerializeField] private float dashCooldown; No need for this anymore!!!
    // private float dashUsageTimer;
    public float dashDir { get; private set; } //making is public get but private set allows it to be public, but NOT VISIBLE in inspector(?)
    private float defaultDashSpeed;
    #endregion                                       

    #region Attack Info
    [Header("AttackInfo")]
    [Tooltip("at how many seconds (since last attack) will the combo series reset back to initial attack animation?")]
    public float attackComboWindow = 2;
    public float attackDashTime = 0.07f;
    public Vector2[] attackDashSpeedModifiers;
    public float counterAttackDuration = 0.2f;

    #endregion

    #region Wallslide and Walljump info
    [Header("Wallslide/Walljump Info")]
    public float wallslideSpeed = 6f;
    [Tooltip("When a player does a wall jump, they will continue to travel horizontally for this many seconds before their xVelocity drops to 0.")]
    public float walljumpMaxDuration = 3f;
    [Tooltip("When a player does a wall jump, they will have to wait this many seconds before they can change their horizontal direction.")]
    public float wallJumpMinDuration = 0.1f;
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
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }

    public PlayerBlackholeState blackholeState { get; private set; }

    public PlayerDeadState deadState { get; private set; }



    #endregion

    public bool isBusy { get; private set; }

    public SkillManager skill {  get; private set; }
    public GameObject sword { get; private set; }

    protected override void Awake()
    {
        base.Awake();
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
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Die");

        
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update()
    {
        base.Update();
        CheckForDashInput();
        stateMachine.currentState.Update();


        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }    
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        // base.SlowEntityBy(_slowPercentage, _slowDuration); //for some reason, we DON'T need the base script here.

        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);

        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);

    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;

        //anim.speed = 1; Don't need this line as long as it's present in the base.ReturnDefaultSpeed (in Entity.cs)

    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword() 
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }


/*    public void SwordCaught()
    {
        stateMachine.ChangeState(idleState);
    }*/

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }


    #region Input Check Methods
    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return; //you cannot dash if you're standing point blank against a wall


        if (Input.GetKey(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill()) 
        {

            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);

        }
    }

    #endregion

   

    #region Animation Triggers
    public void AnimationTrigger()
    {
        stateMachine.currentState.AnimationFinishTrigger();
    }

    public void AnimationMiddleTrigger1()
    {
        stateMachine.currentState.AnimationMiddleTrigger1();
    }
    #endregion

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }






}
