using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, 
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

        player.SetVelocity(xInput * player.moveSpeed * player.airSpeedModifier, rb.velocity.y);

        if (player.IsGroundDetected()) //temporary solution
        {
            AudioManager.instance.PlaySFX(43, null);
            stateMachine.ChangeState(player.idleState);
        }

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallslideState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && PlayerHasSwordInHand() && player.skill.swordThrow.swordUnlocked)
        {
            if (player.skill.swordThrow.CanUseSkill())
            {
                stateMachine.ChangeState(player.aimSwordState);
            }

        }

        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftControl) && Time.timeScale != 0) //if player is removing items, they will not enter attack state!!!
        {
            stateMachine.ChangeState(player.primaryAttackState);
            return;
        }
    }
}
