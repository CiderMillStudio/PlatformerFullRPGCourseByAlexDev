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

        AudioManager.instance.PlaySFX(14, null); //pass null because then we don't do a distance check at all (see audiomanager.cs)
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.instance.StopSFX(14);
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
