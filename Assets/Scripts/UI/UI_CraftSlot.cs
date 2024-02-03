using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot //VERY NEW!!
{


    private void OnValidate()
    {
        if (item != null)
            gameObject.name = "CraftSlot - " + item.data.itemName;
        else
            gameObject.name = "Craft Slot - Empty";
    }

    private void OnEnable()
    {
        if (item != null)
            UpdateSlot(item);
        else
            Debug.Log("There is not an item assigned to this CraftSlot!!! Pls send Halp");
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ItemDataEquipment craftData = item.data as ItemDataEquipment;

        if (Inventory.instance.CanCraft(craftData, craftData.craftingMaterials))
        {
            //play fanfare, happy sounds, particle effects!!!!
        }

    }

}
