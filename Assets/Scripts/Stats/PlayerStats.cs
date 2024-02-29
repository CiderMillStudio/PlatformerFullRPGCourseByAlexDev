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

    public override void TakeDamage(int _damage, Transform _damageSource)
    {
        base.TakeDamage(_damage, _damageSource);

    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

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

    public void CloneDoDamage(CharacterStats _targetStats, float _multiplier, Transform _cloneTransform)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;
        

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multiplier > 0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multiplier);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            //Debug.Log("total crit damage is: " + totalDamage);
        }

        _targetStats.TakeDamage(totalDamage, _cloneTransform);


        DoMagicalDamage(_targetStats, _cloneTransform); //Remove this line if you don't want to apply magic hit on primary attack.

        //if current weapon has fire effect, do fire magical damage, otherwise DON'T!
        //DoMagicalDamage(_targetStats);
    }

}
