using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, 
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



        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);


        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.LeftControl) && Time.timeScale != 0) //if player is removing items, they will not enter attack state!!!
        {
            stateMachine.ChangeState(player.primaryAttackState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q) && player.skill.parry.parryUnlocked)
        { 
            stateMachine.ChangeState(player.counterAttackState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword() && player.skill.swordThrow.swordUnlocked)
        { 
                stateMachine.ChangeState(player.aimSwordState);
        }

        if (Input.GetKeyDown(KeyCode.R) && player.skill.blackhole.blackholeUnlocked)
            stateMachine.ChangeState(player.blackholeState); //NEW

/*        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            Jump(); //moved Jump to Player.cs because StartCoroutine() was needed, but not available in this script.

        }*/


    }

/*    private void Jump() //moved Jump to Player.cs because StartCoroutine() was needed, but is not available in this script.
    {
        AudioManager.instance.PlaySFX(Random.Range(41, 44), null);
        stateMachine.ChangeState(player.jumpState);
    }*/

    /*protected virtual bool HasNoSword()
    {
        if (!player.sword) return true;

        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;

    }*/
}
