using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{

    private bool cloneCreated;


    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine,
        string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!player.skill.parry.parryUnlocked || !player.skill.parry.CanUseSkill()) //maybe delete this code?
        {
            Debug.Log("TRIGGERED, entering idle");
            player.stateMachine.ChangeState(player.idleState);
            return;
        }

        Debug.Log("We've entered CounterAttack State!!");
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessfulCounterAttack", false);
        //we must add this here
        //just in case it doesn't get set back to false from previous success.
    }

    public override void Exit()
    {
        base.Exit();
        player.anim.SetBool("SuccessfulCounterAttack", false); //this was very important
        cloneCreated = false;
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(player.attackCheck.position,
                player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10f; //this just needs to last a long time
                    player.anim.SetBool("SuccessfulCounterAttack", true);
                    
                    player.skill.parry.UseSkill(); //using this parry.UseSkill() method to restore health on Parry (only if this skill is unlocked in the skill tree!) 

                    if (player.skill.parry.restoreUnlocked)
                    {
                        int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * player.skill.parry.restoreHealthPercentage);
                        player.stats.IncreaseHealthBy(restoreAmount);
                    }

                    if (!cloneCreated)
                    {
                        cloneCreated = true;
                        player.skill.parry.MakeMirageOnParry(hit.transform);
                    }


                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            player.stateMachine.ChangeState(player.idleState);

    }



}
