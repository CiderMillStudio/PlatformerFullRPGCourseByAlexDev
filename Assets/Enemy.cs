using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    /*This class inherits from Entity, and contains only information pertinent to ENEMY
     NPCs. I suppose in the future, we may have DIFFERENT types of enemies, so perhaps 
    eventually we will have scripts for Enemy sub-types that inherit from THIS class!*/

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move Info")]
    public float moveSpeed = 2f;
    public float idleTime = 2f;

    [Header("Attack Info")]
    public float battleRange = 6f;
    public float attackDistance = 1f; 
    public float attackSpeedModifier = 2f;
    public Transform BattleRangeCheck;
    public Transform AttackDistanceCheck;
    
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
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

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(BattleRangeCheck.position, 
        new Vector2(facingDir, 0), battleRange, whatIsPlayer);

    public virtual RaycastHit2D IsPlayerInRange() => Physics2D.Raycast(AttackDistanceCheck.position,
    new Vector2(facingDir, 0), attackDistance, whatIsPlayer);




}
