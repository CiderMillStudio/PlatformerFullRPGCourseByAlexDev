using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDirection;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, 
        string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;

        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;


            if (enemy.IsPlayerInRange())
            {
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }

        else if (stateTimer < 0 || Vector2.Distance(enemy.transform.position, player.transform.position) > enemy.stopAttackDistance)
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        
        

        if (player.position.x > enemy.transform.position.x)
        {
            moveDirection = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDirection = -1;
        }

        enemy.SetVelocity(enemy.attackSpeedModifier * enemy.moveSpeed * moveDirection, 
            rb.velocity.y);


    }

    private bool CanAttack()
    {
        if (enemy.lastTimeAttacked + enemy.attackCooldown < Time.time)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
