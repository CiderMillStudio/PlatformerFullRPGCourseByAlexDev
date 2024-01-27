using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;

    protected Enemy enemyBase;

    private string animBoolName;

    protected float stateTimer;
    protected bool triggerCalled;
    protected bool middleTrigger1Called;
    protected Rigidbody2D rb;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, 
        string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.enemyBase = _enemyBase;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;


    }

    public virtual void Enter()
    {
        rb = enemyBase.rb;

        triggerCalled = false;
        enemyBase.anim.SetBool(animBoolName, true);
        if (stateTimer < 0)
        {
            stateTimer = 0;
        }
    
    }

    public virtual void Exit()
    {
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public virtual void AnimationMiddleTrigger1()
    { middleTrigger1Called = true; }



}
