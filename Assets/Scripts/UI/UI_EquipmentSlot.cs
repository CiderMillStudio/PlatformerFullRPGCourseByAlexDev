using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (itemImage.sprite == null)
            return;

        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment); //added this line at 6:08pm because we want to only use
                                                                    //"AddItem" when we definitely will need it. We want to
                                                                    //save "UnequipItem" for situations where items get
                                                                    //removed, and maybe even when we die we "unequip items"
                                                                    //but don't want them to go back into our
                                                                    //inventory (because we drop them!)
        CleanUpSlot();
        
    }
}
