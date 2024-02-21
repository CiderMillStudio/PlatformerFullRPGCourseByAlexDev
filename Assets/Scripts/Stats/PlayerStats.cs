using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    private PlayerItemDrop myPlayerItemDropSystem;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        myPlayerItemDropSystem = GetComponent<PlayerItemDrop>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();

        player.Die();
        myPlayerItemDropSystem.GenerateDropUponDeath();
    }

    protected override void DecreaseHealthBy(int _damage) 
    {
        base.DecreaseHealthBy(_damage);

        ItemDataEquipment currentArmor = Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null && Inventory.instance.CanUseArmor())
        {
            currentArmor.Effect(player.transform);
        }
    }


    public override void OnEvasion(Transform _enemyTransform)
    {
        player.skill.dodge.CreateMirageOnDodge(_enemyTransform);
    }

}
