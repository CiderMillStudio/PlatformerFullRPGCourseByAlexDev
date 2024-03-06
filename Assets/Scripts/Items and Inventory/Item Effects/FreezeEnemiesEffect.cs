using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/ItemEffect/Freeze Enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float duration;


    public override void ExecuteEffect(Transform _respawnTransform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats.currentHealth > playerStats.GetMaxHealthValue() * 0.8f)
            return;
        

/*        if (!Inventory.instance.CanUseArmor())
            return;*/

        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(_respawnTransform.position, 3);

        foreach (var hit in colliders)
        {
                hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
