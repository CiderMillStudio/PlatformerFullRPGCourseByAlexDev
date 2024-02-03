using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's dropped items")]
    [Range(0,100)]
    [SerializeField] private float chanceToLoseEquipment;
    [Range(0, 100)]
    [SerializeField] private float chanceToLoseMaterials;


    public override void GenerateDrop() //My Generate Drop (Player version) is rather custom, so if things break later on, suspect the GEnerate Drop
    {
        Inventory inventory = Inventory.instance;

        Debug.Log("Custom Generate Drop, BEWARE!");

        //list of equipment (via inventory)
        List<InventoryItem> currentEquipment = new List<InventoryItem>(); 
        List<InventoryItem> currentStash = new List<InventoryItem>();
        
        foreach (InventoryItem inventoryItem in inventory.GetEquipmentList())
        {
            currentEquipment.Add(inventoryItem);
        }

        foreach (InventoryItem stashItem in inventory.GetStashList())
        {
            currentStash.Add(stashItem);
        }

        //for each item we'll check if we should lose it

        foreach (InventoryItem item in currentEquipment)
        {
            if (Random.Range(0,100) <= chanceToLoseEquipment)
            {
                DropItem(item.data);
                inventory.UnequipItem(item.data as ItemDataEquipment);
            }
        }

        foreach (InventoryItem item in currentStash)
        {
            if (Random.Range(0, 100) <= chanceToLoseMaterials)
            {
                
                for (int i = 0; i < Random.Range(0, item.stackSize); i++)
                {
                        inventory.RemoveItem(item.data);
                        DropItem(item.data);
                }
                    
            }
        }
    }
}
