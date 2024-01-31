using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData; //itemData is a scriptable object data type that defines the properties of specific items

    private void OnValidate() //this is called anytime you change anything in the object in Unity
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.name;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.instance.AddItem(itemData); //NEW!!

            Destroy(gameObject);
        }
    }


}
