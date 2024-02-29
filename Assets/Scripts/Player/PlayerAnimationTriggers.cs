using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private bool hitEnemy;
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
        
        hitEnemy = false;

        Collider2D[] colliders = 
            Physics2D.OverlapCircleAll(player.attackCheck.position, 
                player.attackCheckRadius);



        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hitEnemy = true;
                EnemyStats _target = hit.GetComponent<EnemyStats>();

                if (_target != null)
                    player.stats.DoDamage(_target);
                
                //hit.GetComponent<Enemy>().Damage();
                
                // inventory, get weapon, call item effect:
                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
            }
        }

        if (!hitEnemy)
            AudioManager.instance.PlaySFX(2, player.transform);
        else
            AudioManager.instance.PlaySFX(1, player.transform);



        // we chose to implement item effects HERE instead of DoDamage()
        // (in characterStats) because DoDamage is called even for abilities,
        // not just for weapon damage. Since we want to exclusively call item effects when
        // WEAPONS are used, we'll call ItemEffect here.

    }
    
    void ThrowSword()
    {
        SkillManager.instance.swordThrow.CreateSword();
    }


}
