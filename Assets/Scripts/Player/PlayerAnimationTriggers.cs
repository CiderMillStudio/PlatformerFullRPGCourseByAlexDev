using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    void AnimationMiddleTrigger1()
    {
        player.AnimationMiddleTrigger1();
    }

    void AnimationDamageTrigger()
    {
        Collider2D[] colliders = 
            Physics2D.OverlapCircleAll(player.attackCheck.position, 
                player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }

    }
    
    void ThrowSword()
    {
        SkillManager.instance.swordThrow.CreateSword();
    }
}
