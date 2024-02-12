using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWalljumpState : PlayerState
{
    public PlayerWalljumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        
        
        stateTimer = player.walljumpMaxDuration;//this is how long we'll be in the wallJump state,
                           //then we'll transfer to PlayerAirState.

        player.SetVelocity(player.moveSpeed * -player.facingDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && xInput != player.facingDir && 
            xInput != 0 && stateTimer < player.walljumpMaxDuration - 
            player.wallJumpMinDuration)
        { 
            stateMachine.ChangeState(player.airState);
            return;
        }  
        
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }



        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallslideState);
        }
            
        if (stateTimer < 0)
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSwordState);
    }
}
