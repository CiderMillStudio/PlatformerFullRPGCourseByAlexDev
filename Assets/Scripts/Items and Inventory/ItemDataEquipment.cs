using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Equipment Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData //thus, ItemDataEquipment is also a S.O., like ItemData itself
{
    public EquipmentType equipmentType;

    public float itemCooldown;

    public ItemEffect[] itemEffects;

    #region Stats (The Grand Encyclopedia of Stats)
    #region Major Stats
    [Header("Major Stats")]

    public int strength; //1 pt increase damage by 1 and crit.power by 1%
    public int agility; //1 pt increase evasion by 1% and crit.chance by 1%
    public int intelligence; //1 pt increase magic damage by 1 and magic resistance by 1%.. or 3%? (just FYI We won't be making a bunch of spells)
    public int vitality; //1 pt increase health by 3 or 5 points?

    #endregion

    #region Offensive Stats
    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;   //default value 150 (%)

    #endregion

    #region Defensive Stats
    [Header("Defensive Stats")]
    public int maxHealth;
    public int armor;
    public int evasion;
    public int magicResistance;

    #endregion

    #region Magic Stats
    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightningDamage;

    #endregion

    #endregion

    [Header("Craft Requirements")] 
    public List<InventoryItem> craftingMaterials; //this is the recipe for any given equipment item

    private int descriptionLength;


    public void Effect(Transform _respawnTransform)
    {
        foreach (ItemEffect itemEffect in itemEffects)
        {
            itemEffect.ExecuteEffect(_respawnTransform);
        }
    }
    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);
        
        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.lightningDamage.AddModifier(lightningDamage);
        playerStats.iceDamage.AddModifier(iceDamage);  
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.lightningDamage.RemoveModifier(lightningDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);

    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;

        AddItemDescription(strength, "Strength");  // 3
        AddItemDescription(agility, "Agility");  //2
        AddItemDescription(vitality, "Vitality");
        AddItemDescription(intelligence, "Intelligence");

        AddItemDescription(damage, "Damage");
        AddItemDescription(critChance, "Crit Chance");
        AddItemDescription(critPower, "Crit Power");

        AddItemDescription(armor, "Armor");  //5
        AddItemDescription(maxHealth, "Max Health");
        AddItemDescription(magicResistance, "Magic Resistance");
        AddItemDescription(evasion, "Evasion");

        AddItemDescription(iceDamage, "Ice Damage");
        AddItemDescription(fireDamage, "Fire Damage");
        AddItemDescription(lightningDamage, "Shock Damage");

        if (descriptionLength <= 5) //it's 3
        {
            for (int i = 0; i <= 5 - descriptionLength; i++) 
            {
                sb.AppendLine();
                sb.Append(""); //in this case, we'd add 2 blank lines. This way, each item Tooltip box's description section is 5 lines long (except for really powerful items with more than 5 stat modifiers)
            }
        }


        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name) //value is the modifier of a specific stat, and the NAME is the name of the stat
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
            {
                sb.AppendLine();
            }

            if (_value > 0)
            {
                sb.Append("+" + _value + " " + _name);
            }

            descriptionLength++;
        }
    }
}
