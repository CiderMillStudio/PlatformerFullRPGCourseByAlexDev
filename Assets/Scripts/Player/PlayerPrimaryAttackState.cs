using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int comboCounter; //  2/5/2023 just set this to PUBLIC from PRIVATE! (so we can access it from IceAndFireEffect.cs)

    private float lastTimeAttacked; //how long ago was the last attack?

    private bool inAir;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, 
        string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //AudioManager.instance.PlaySFX(2);
        xInput = 0; /*WE ADDED THIS IN LECTURE 59 due to a bug, which fixed Alex's 
                     attack direction. I don't think it will have any impact on my 
                        code (I never had a bug!) but I don't think this could possibly 
                        hurt in anyway. */

        if (player.IsGroundDetected())
            player.SetZeroVelocity();
        else
        {
            inAir = true;
            player.SetVelocity(rb.velocity.x, rb.velocity.y);
        }

        if (comboCounter > 2 || Time.time - lastTimeAttacked >= player.attackComboWindow)
        {
            comboCounter = 0;
            player.anim.SetInteger("attackComboCounter", comboCounter);
        }
    }

    public override void Exit()
    {
        base.Exit();
        comboCounter++;
        player.anim.SetInteger("attackComboCounter", comboCounter);
        lastTimeAttacked = Time.time; //welp, this is a new time function!!
        player.StartCoroutine("BusyFor", 0.1f); //0.1f seconds is enough to                                              
    }                                           //block unwanted movement!

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected() && inAir)
        {
            inAir = false;
            player.SetZeroVelocity();
        }
        else if (!player.IsGroundDetected() && inAir)
        {
            player.SetVelocity(rb.velocity.x, rb.velocity.y);
        }

        if (middleTrigger1Called)
        {
            middleTrigger1Called = false;
            player.SetVelocity(player.moveSpeed *
                player.attackDashSpeedModifiers[comboCounter].x * player.facingDir,
                player.attackDashSpeedModifiers[comboCounter].y);
            stateTimer = player.attackDashTime;
        }

        if (triggerCalled)
        {
            if (xInput != 0 && player.facingDir != xInput)
                player.Flip();

            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer < 0 && player.IsGroundDetected())
            player.SetVelocity(0, 0);

        if (player.stats.isDead)
        {
            stateMachine.ChangeState(player.deadState);
        }
    }
}
