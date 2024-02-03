using System.Collections;
using System.Collections.Generic;
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
        myPlayerItemDropSystem.GenerateDrop();
    }

}
