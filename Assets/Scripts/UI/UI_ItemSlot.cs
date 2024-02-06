using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //!!!
using TMPro; //!!!
using UnityEngine.EventSystems;
using Unity.VisualScripting;   //Don't forget these!!

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler
{

    [SerializeField] protected Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Image defaultSlotImage;
    protected PlayerItemDrop playerItemDropSystem;


    public InventoryItem item;

    private void Start()
    {
        //need PlayerItemDrop in the event the player decides to discard items, which cause them to be dropped into the world
        playerItemDropSystem = PlayerManager.instance.player.GetComponent<PlayerItemDrop>(); 
    }

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

    public void SetDefaultSlotImage() //Wady's creativity might get the best of him.
    {
        
        itemImage.sprite = defaultSlotImage.sprite;
        itemImage.color = defaultSlotImage.color;
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (itemImage.sprite == null)
            return;


        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerItemDropSystem.SingleItemDrop(item.data);
            Inventory.instance.RemoveItem(item.data);
            return;
        }
        
        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);            

    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

}
