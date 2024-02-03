using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !PlayerManager.instance.player.stats.isDead)
        {
            myItemObject.PickupItem();
        }
    }
}
