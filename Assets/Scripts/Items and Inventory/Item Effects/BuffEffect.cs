using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage, 
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightningDamage
}
[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/ItemEffect/Buff Effect")]
public class BuffEffect : ItemEffect
{
    [SerializeField] private StatType buffType;
    private PlayerStats stats;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;

    public override void ExecuteEffect(Transform _respawnTransform)
    {
        //delete base!

        stats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        //stats.IncreaseStatBy(buffAmount, buffDuration, stats.armor) 
        //Instead of choosing which stat you want to increase here, you we can make a long, tedious function
        //(and an enum) to help us SET the stat in BuffEffect's Scriptable Object entity!!

        stats.IncreaseStatBy(buffAmount, buffDuration, StatToModify());

        
    }

    private Stat StatToModify()
    {
        if (buffType == StatType.strength)
            return stats.strength;
        else if (buffType == StatType.agility)
            return stats.agility;
        else if (buffType == StatType.intelligence)
            return stats.intelligence;
        else if (buffType == StatType.vitality)
            return stats.vitality;
        else if (buffType == StatType.damage)
            return stats.damage;
        else if (buffType == StatType.critChance)
            return stats.critChance;
        else if (buffType == StatType.critPower)
            return stats.critPower;
        else if (buffType == StatType.health)
            return stats.maxHealth;
        else if (buffType == StatType.armor)
            return stats.armor;
        else if (buffType == StatType.magicResistance)
            return stats.magicResistance;
        else if (buffType == StatType.evasion)
            return stats.evasion;
        else if (buffType == StatType.fireDamage)
            return stats.fireDamage;
        else if (buffType == StatType.iceDamage)
            return stats.iceDamage;
        else if (buffType == StatType.lightningDamage)
            return stats.lightningDamage;

        else
            return null;
    }    
}
