using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallslideState : PlayerState
{

    private int slideSfxIndex;
    public PlayerWallslideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        slideSfxIndex = Random.Range(41, 43);
        AudioManager.instance.FadeInSfxVolume(slideSfxIndex, 60f) ;
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(slideSfxIndex);
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlaySFX(44, null);
            AudioManager.instance.PlaySFX(Random.Range(38, 41), null);
            stateMachine.ChangeState(player.walljumpState);
            return;
        }

        if (!player.IsWallDetected() || player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            return;

        }

        WallslideMovement();



    }


    private void WallslideMovement()
    {
        if (yInput == -1)
            player.SetVelocity(xInput, -3.5f * player.wallslideSpeed);

        else if (yInput == 1)
            player.SetVelocity(xInput, -0.2f * player.wallslideSpeed);

        else
            player.SetVelocity(xInput, -player.wallslideSpeed);
    }
}
