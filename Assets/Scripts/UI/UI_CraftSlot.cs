using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_CraftSlot : UI_ItemSlot
{

    [SerializeField] private int defaultFontSize = 24;
    private void OnValidate()
    {
        if (item.data != null)
            gameObject.name = "CraftSlot - " + item.data.itemName;
        else
            gameObject.name = "Craft Slot - Empty";
    }

    private void OnEnable()
    {
        if (item != null)
        {
            UpdateSlot(item);
        }
        else
            Debug.Log("There is not an item assigned to this CraftSlot!!! Pls send Halp");
    }


    /*    protected override void Start()
        {
            base.Start();
        }*/ //Just got rid of this!

    public void SetupCraftSlot(ItemDataEquipment _data)
    {
        Debug.Log("Setting up craft slot!");
        
        if (_data == null)
            return;

        itemText.fontSize = defaultFontSize;
        item.data = _data;
        itemImage.sprite = _data.icon;
        itemText.text = _data.itemName;

        if (itemText.text.Length >= 12)
        {
            itemText.fontSize = defaultFontSize * 0.7f;
        }

        if (item.data != null)
            gameObject.name = "CraftSlot - " + item.data.itemName;
        else
            gameObject.name = "Craft Slot - Empty";
    }



    public override void OnPointerDown(PointerEventData eventData)
    {
        /*        ItemDataEquipment craftData = item.data as ItemDataEquipment;

                if (Inventory.instance.CanCraft(craftData, craftData.craftingMaterials))
                {
                    //play fanfare, happy sounds, particle effects!!!!
                }*/

        ui.craftWindow.SetupCraftWindow(item.data as ItemDataEquipment);

    }

}
