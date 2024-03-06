using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, 
        string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed * player.airSpeedModifier, rb.velocity.y);

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && PlayerHasSwordInHand() && player.skill.swordThrow.swordUnlocked)
        {
            if (player.skill.swordThrow.CanUseSkill())
            {
                stateMachine.ChangeState(player.aimSwordState);
            }

        }
    }
}
