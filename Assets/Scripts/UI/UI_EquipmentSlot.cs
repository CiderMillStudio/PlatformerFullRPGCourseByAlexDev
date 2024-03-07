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
        if (itemImage.sprite == null) //wady's brilliance
            return;

        if (!Inventory.instance.CanAddItem())
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerItemDropSystem.SingleItemDrop(item.data, true);
            Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
            CleanUpSlot();
            return;
        }

/*        if (Input.GetKey(KeyCode.E) && slotType == EquipmentType.Flask)
        {
            ItemDataEquipment equippedFlask = Inventory.instance.GetEquipment(EquipmentType.Flask);
            equippedFlask.Effect(playerItemDropSystem.GetComponent<Player>().transform); //just an easy way to get to player's transform
            Inventory.instance.UnequipItem(equippedFlask);
            CleanUpSlot();
            return;
        }*/  //The commented out code block above is Wady's
         //custom system for CONSUMING flasks. Alex uses a completely 
        //different mechanic, such that flasks aren't consumed, but can be used repeatitively based on a cooldown.

        Inventory.instance.UnequipItem(item.data as ItemDataEquipment);
        //Inventory.instance.AddItem(item.data as ItemDataEquipment); //added this line at 6:08pm because we want to only use
                                                                    //"AddItem" when we definitely will need it. We want to
                                                                    //save "UnequipItem" for situations where items get
                                                                    //removed, and maybe even when we die we "unequip items"
                                                                    //but don't want them to go back into our
                                                                    //inventory (because we drop them!)
        ui.itemTooltip.HideToolTip();

        AudioManager.instance.PlaySFX(7, null);

        CleanUpSlot();
        
    }
}
