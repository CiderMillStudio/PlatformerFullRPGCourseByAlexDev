using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //!!!
using TMPro; //!!!
using UnityEngine.EventSystems;
using Unity.VisualScripting;   //Don't forget these!!

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] private Image defaultSlotImage;

    private RectTransform thisSlotRectTransform;

    protected PlayerItemDrop playerItemDropSystem;
    

    protected UI ui;

    public InventoryItem item;


    protected virtual void Start()
    {
        //need PlayerItemDrop in the event the player decides to discard items, which cause them to be dropped into the world
        playerItemDropSystem = PlayerManager.instance.player.GetComponent<PlayerItemDrop>(); 
        ui = GetComponentInParent<UI>();
        thisSlotRectTransform = GetComponent<RectTransform>();
    }

    public void UpdateSlot(InventoryItem _newItem) //Updates the slot (its image and its itemAmount integer value) that
                                                   //this instance of UI_ItemSlot is responsible for managing.
    {
        item = _newItem;
        itemImage.color = Color.white;

        if (item != null)
        {
            if (item.data != null)
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

        ui.itemTooltip.HideToolTip();

        AudioManager.instance.PlaySFX(7, null);

    }

    public void CleanUpSlot()
    {
        item = null;
        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemTooltip?.ShowToolTip(item.data as ItemDataEquipment);


        AudioManager.instance.PlaySFX(36, null);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemTooltip?.HideToolTip();
    }

    private void Update()
    {
        if (ui.itemTooltip != null)
        {

            if (ui.itemTooltip.isActiveAndEnabled && ui.itemTooltip.itemTooltipRectTransform != null)
            {
                ui.itemTooltip.itemTooltipRectTransform.position = Input.mousePosition + new Vector3(0, 0.10f * Screen.height);
            }

        }
    }
}
