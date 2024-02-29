using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, 
        string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.dash.CreateCloneOnDash();
        // ^Was: SkillManager.instance.clone.CreateClone(player.transform);
        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(player.moveSpeed * player.facingDir, rb.velocity.y);
        player.skill.dash.CreateCloneOnArrival();

    }

    public override void Update()
    {
        base.Update();

        if (xInput == 0 ) //I added this line on my own...
            player.SetVelocity(player.dashSpeed * player.dashDir, 0);//I added this line on my own...
        else//I added this line on my own...
            player.SetVelocity(player.dashSpeed * xInput, 0); //wady edited this code, it used to be: player.SetVelocity(player.dashSpeed * player.dashDir, 0). BEfore my edits, there was not an if/else if statement. just the SetVelocity() method!

        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.swordThrow.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordState);
    }
}
