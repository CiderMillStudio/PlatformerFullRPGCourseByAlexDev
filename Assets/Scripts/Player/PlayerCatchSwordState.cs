using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    private float modifiedSwordReturnImpact;

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;
        if (!player.IsGroundDetected()) { modifiedSwordReturnImpact = player.swordReturnImpact * 3f; }
        else { modifiedSwordReturnImpact = player.swordReturnImpact; }

        AudioManager.instance.PlaySFX(64, null);
        
        if (player.transform.position.x > sword.position.x && player.facingDir == 1) //HONESTLY alex's solution is better than mine!
        {
            player.Flip();
        }
        if (player.transform.position.x < sword.position.x && player.facingDir == -1)
        {
            player.Flip();
        }
        rb.velocity = new Vector2(modifiedSwordReturnImpact * -player.facingDir, rb.velocity.y);//we don't want to flip the character, so we don't use SetVelocity();
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", 0.1f); 
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState); //Once again, HONESTLY, Alex's solution is better than mine!
                                                        //Instead of making a bunch of "SwordCaught" scripts in playeranimationtriggers.cs and in other scripts,
                                                        //just use the animationTrigger function!
        }
    }
}
