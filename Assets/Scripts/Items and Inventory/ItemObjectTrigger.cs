using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void Start()
    {
        Invoke("EnableCanBePickedUp", 2f); //this is a good safeguard in the event that the second Invoke never actually fires
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !PlayerManager.instance.player.stats.isDead)
        {
            myItemObject.PickupItem();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (this != null)
            Invoke("EnableCanBePickedUp", 0.5f);
    }

    private void EnableCanBePickedUp()
    {
        
        //if (this != null)
            myItemObject.canBePickedUp = true;
    }
}
