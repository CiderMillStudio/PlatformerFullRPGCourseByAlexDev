using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunnedState stunnedState { get; private set; }

    public SkeletonDeadState deadState { get; private set; }


    #endregion

    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new SkeletonMoveState(this, stateMachine, "isMoving", this);
        battleState = new SkeletonBattleState(this, stateMachine, "isMoving", this);
        attackState = new SkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SkeletonStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SkeletonDeadState(this, stateMachine, "Idle", this); //it doesn't matter which animBoolNAme you use, it's just gonna use the lastAnimBoolName

        
        /*We're gonna use "isMoving" for Battle State because when Battle state
         Activates, the first thing the skeleton does is start moving more 
         quickly towards the player !! */
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

    }

    public override bool CanBeStunned() //only EnemySkeleton has info about its states,
    {                                       //that's why we override here!
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState); //<-- this code would be inaccessible
                                                    //in our Enemy.cs base script!
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);

    }
}
