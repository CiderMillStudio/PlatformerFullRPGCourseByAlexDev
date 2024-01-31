using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] // VERY IMPORTANT
public class InventoryItem //NO INHERITANCE FROM MONOBEHAVIOR
    //To my knowledge, an "InventoryItem" represents a "slot" in an inventory UI, like the boxes in stardew valley's inventory, or minecraft.
{
    public ItemData data;  //Item type (ItemData is a Scriptable Object)
    public int stackSize; //This is the number of items in the given itemSlot

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        //TODO: add to stack
        AddStack();
    }

    public void AddStack() => stackSize++;

    public void RemoveStack() => stackSize--;
}
