using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, 
        string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.currentState != this)
        {
            return;
        }

        else if (xInput == 0 || player.IsWallDetected() && xInput == player.facingDir)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        else
            player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);



        
        
    }
}