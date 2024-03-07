using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{
    /*This class inherits from Entity, and contains only information pertinent to ENEMY
     NPCs. I suppose in the future, we may have DIFFERENT types of enemies, so perhaps 
    eventually we will have scripts for Enemy sub-types that inherit from THIS class!*/

    [SerializeField] protected LayerMask whatIsPlayer;


    [Header("Stun Info")]
    public float stunDuration = 1f;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Move Info")]
    public float moveSpeed = 2f;
    public float idleTime = 2f;
    public float battleTime = 4f;
    public float stopAttackDistance = 10f;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float battleRange = 6f;
    public float attackDistance = 1f; 
    public float attackSpeedModifier = 2f;
    public Transform BattleRangeCheck;
    public Transform AttackDistanceCheck;
    public float attackCooldown = 1.2f;
    [HideInInspector] public float lastTimeAttacked;
    
    public EnemyStateMachine stateMachine { get; private set; }

    public string lastAnimBoolName {  get; private set; } //whenever the enemy changes animation states,
                                                          //lastAnimBoolName changes accordingly so that later when we enter
                                                          //deadState, we can stop the animation that was played last.

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        // base.SlowEntityBy(_slowPercentage, _slowDuration); As in player's override version, we don't need the base
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);

    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;

        //as in player's version of the override, we don't need to reset anim.speed because that happens in the base version.
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed; //because different enemy types will have different 
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimeCoroutine(_duration));

    protected virtual IEnumerator FreezeTimeCoroutine(float _seconds)
    {
        FreezeTime(true);
        
        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow() // doing public virtual void just in case we need to override it later.
    {
        canBeStunned = true;
        counterImage.SetActive(false); //eventually should delete this image altogethter, but i'm feeling lazy today lol
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion


    public virtual bool CanBeStunned() //since "protected", we may override it in
                                           //EnemySkeleton.cs!
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.red;

        Gizmos.DrawLine(BattleRangeCheck.position,
            new Vector3(BattleRangeCheck.position.x + facingDir * battleRange,
            BattleRangeCheck.position.y));
        Gizmos.DrawLine(AttackDistanceCheck.position,
            new Vector3(AttackDistanceCheck.position.x + facingDir * attackDistance,
                AttackDistanceCheck.position.y));
    }

    public void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public void AnimationMiddleTrigger1() => stateMachine.currentState.AnimationMiddleTrigger1();
 

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(BattleRangeCheck.position, 
        new Vector2(facingDir, 0), battleRange, whatIsPlayer);

    public virtual RaycastHit2D IsPlayerInRange() => Physics2D.Raycast(AttackDistanceCheck.position,
    new Vector2(facingDir, 0), attackDistance, whatIsPlayer);




}
