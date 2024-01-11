using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked; //how long ago was the last attack?
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, 
        string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        xInput = 0; /*WE ADDED THIS IN LECTURE 59 due to a bug, which fixed Alex's 
                     attack direction. I don't think it will have any impact on my 
                        code (I never had a bug!) but I don't think this could possibly 
                        hurt in anyway. */

        player.SetZeroVelocity();

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

        if (stateTimer < 0)
            player.SetVelocity(0, 0);
    }
}
