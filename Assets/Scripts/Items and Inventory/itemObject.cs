using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData; //itemData is a scriptable object data type that defines the properties of specific items
    public bool canBePickedUp = false;
    private void OnValidate() //this is called anytime you change anything in the object in Unity
    {
        SetupVisuals();
    }
     
    private void SetupVisuals()
    {
        if (itemData != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item Object - " + itemData.name;
        }
        else
        {
            gameObject.name = "Item Object - Empty";
        }
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
       
        itemData = _itemData;
        
        rb.velocity = _velocity;


        SetupVisuals();
    }


    /*    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>() != null)
            {
                PickupItem();
            }
        }*/   //We removed this OnTriggerEnter2D on 2/2/2024, replacing the functionality in ItemObjectTrigger;

    public void PickupItem()
    {
        if (canBePickedUp)
        {
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
