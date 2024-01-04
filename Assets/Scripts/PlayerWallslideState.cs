using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallslideState : PlayerState
{
    public PlayerWallslideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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


        if (yInput == -1)
            player.SetVelocity(xInput, -3.5f * player.wallslideSpeed);

        else if (yInput == 1)
            player.SetVelocity(xInput, -0.2f * player.wallslideSpeed);

        else
            player.SetVelocity(xInput, -player.wallslideSpeed);

        if (!player.IsWallDetected() || player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
