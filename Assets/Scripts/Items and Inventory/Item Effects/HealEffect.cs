using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/ItemEffect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercent;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        //get player stats
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //determine how much to heal by
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercent);

        //heal player
        playerStats.IncreaseHealthBy(healAmount);
    }
}
