using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/*public enum StatType //Moved this to CharacterStats.cs
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
}*/
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

        stats.IncreaseStatBy(buffAmount, buffDuration, stats.StatOfType(buffType)); //new!! (was just StatToModify())

        if (buffType == StatType.strength || buffType == StatType.critChance || buffType == StatType.critPower || buffType == StatType.damage || buffType == StatType.agility)
            AudioManager.instance.PlaySFX(87, null);
        else if (buffType == StatType.intelligence || buffType == StatType.magicResistance || buffType == StatType.fireDamage || buffType == StatType.iceDamage || buffType == StatType.lightningDamage)
            AudioManager.instance.PlaySFX(88, null);
        else if (buffType == StatType.vitality || buffType == StatType.health || buffType == StatType.armor || buffType == StatType.evasion)
            AudioManager.instance.PlaySFX(89, null);

    }

    /*private Stat StatToModify() //Moved this to CharacterStats.cs
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
    }    */
}
