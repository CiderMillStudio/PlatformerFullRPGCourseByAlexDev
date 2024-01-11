using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    EnemySkeleton enemy;

    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, 
        string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunDuration;
        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x,
            enemy.stunDirection.y);

        enemy.fx.InvokeRepeating("RedColorBlink", 0, 0.1f);
        
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("StopRedColorBlink", 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            enemy.stateMachine.ChangeState(enemy.idleState);
    }


}
