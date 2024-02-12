using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    protected Rigidbody2D rb;


    protected float xInput;
    protected float yInput;

    private string animBoolName;

    protected float stateTimer;

    protected bool triggerCalled; //refers to things like animation triggers
    protected bool middleTrigger1Called;


    public PlayerState(Player _player, PlayerStateMachine _stateMachine, 
        string _animBoolName) 
        //this is called a constructor... but what does it do? idk...
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
        rb = player.rb;
        triggerCalled = false;
        middleTrigger1Called = false;
    }

    public virtual void Update() 
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

    public virtual void AnimationMiddleTrigger1()
    {
        middleTrigger1Called = true;
    }

    protected virtual bool HasNoSword()
    {
        if (!player.sword) return true;

        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;

    }
}
