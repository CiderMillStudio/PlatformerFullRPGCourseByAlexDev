using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level Details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = 0.4f; //0.4 indicates a 40% increase in stat value on leveling up

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();
        
        myDropSystem = GetComponent<ItemDrop>();


    }

    private void ApplyLevelModifiers()
    {
        foreach (Stat stat in listOfStats)
        {
            Modify(stat);
        }
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++) //we start to increase stats only when enemy reaches level 2, so do nothing for i = 0!
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

        
    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();

        myDropSystem.GenerateDrop();
    }
}
