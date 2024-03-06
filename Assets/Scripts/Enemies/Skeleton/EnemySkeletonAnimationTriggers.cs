using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();
    public void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    void AnimationMiddleTrigger1()
    {
        enemy.AnimationMiddleTrigger1();
    }

    public void AnimationDamageTrigger()
    {
        Collider2D[] colliders = 
            Physics2D.OverlapCircleAll(enemy.attackCheck.position, 
                enemy.attackCheckRadius);

        foreach (var hit in colliders) 
        { 
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();

                enemy.stats.DoPhysicalDamage(_target);

                //hit.GetComponent<Player>().Damage();
            }
        }
    }

    private void OpenCounterAttackWindow()
    {
        enemy.OpenCounterAttackWindow();
    }


    private void CloseCounterAttackWindow()
    {
        enemy.CloseCounterAttackWindow();
    }

    private void SkeletonAttackSFX()
    {
        AudioManager.instance.PlaySFX(Random.Range(107, 110), transform);
    }

}
