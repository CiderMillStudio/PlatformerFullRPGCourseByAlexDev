using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //!!!
using TMPro;   //Don't forget these!!

public class UI_ItemSlot : MonoBehaviour
{

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;


    public InventoryItem item;


    public void UpdateSlot(InventoryItem _newItem) //Updates the slot (its image and its itemAmount integer value) that
                                                   //this instance of UI_ItemSlot is responsible for managing.
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }
}
