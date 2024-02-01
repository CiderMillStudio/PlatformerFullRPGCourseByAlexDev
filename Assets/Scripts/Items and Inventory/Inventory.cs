using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;



public class Inventory : MonoBehaviour
{
    public static Inventory instance; // PUBLIC STATIC


    public List<InventoryItem> inventory; //was inventoryItems
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; //Dictionary = Paired {Key + Value} data types

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent; //NEw
    [SerializeField] private Transform equipmentSlotParent;

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots; //NEW
    private UI_EquipmentSlot[] equipmentSlots;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();
                
        inventoryItemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>(); //Creates an array of all the itemSlots in the inventory UI
        stashItemSlots = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();

    }
    
    public void EquipItem(ItemData _item) //let's pretent _item = wooden sword's ItemData.
    {

        ItemDataEquipment newEquipment = _item as ItemDataEquipment; //WOWW!!!! AS IS SOOO COOL!!!!
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemDataEquipment oldEquipment = null; //in our pretend scenario, oldEquipment could be a stick

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> equippedItem in equipmentDictionary)
        {

            if (equippedItem.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = equippedItem.Key;
            }

        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment); //"AddItem" refers to adding the oldEquipment (the stick) back to our inventory, from its equipment slot
        }
        

            equipment.Add(newItem);
            equipmentDictionary.Add(newEquipment, newItem);
            newEquipment.AddModifiers();
            RemoveItem(newEquipment); //"RemoveItem" refers to Removing the item from our inventory, because we're moving it INTO an equipment slot

            UpdateSlotUI();

    }

    public void UnequipItem(ItemDataEquipment itemToRemove) //Changed this method to PUBLIC from PRIVATE 2/1/24 at 6:02pm
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            //AddItem(itemToRemove); //ADDED THIS LINE 2/1/24 at 6:05pm. Deleted this line at 6:07pm
            equipment.Remove(value);
            itemToRemove.RemoveModifiers();
            equipmentDictionary.Remove(itemToRemove);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) //This is just for debugging purposes, delete later.
        {
            Debug.Log(inventory.Count - 1);
            ItemData newItem = inventory[inventory.Count - 1].data;

            RemoveItem(newItem);

        }    

        if (Input.GetKeyDown(KeyCode.K))
        {
            ItemData newStashItem = stash[stash.Count - 1].data;

            RemoveItem(newStashItem);
        }
    }

    private void UpdateSlotUI()
    {
        
        
        for (int i = 0; i < inventoryItemSlots.Length; i++)
        {
            inventoryItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < stashItemSlots.Length; i++)
        {
            stashItemSlots[i].CleanUpSlot();
        }
        for (int i = 0; i < equipmentSlots.Length; i++) //Maybe delete?
        {
            equipmentSlots[i].CleanUpSlot();
        }


        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> equippedItem in equipmentDictionary)
            {

                if (equippedItem.Key.equipmentType == equipmentSlots[i].slotType)
                {
                    equipmentSlots[i].UpdateSlot(equippedItem.Value);
                }

            }
        }




        for (int i = 0; i < inventory.Count; i++) //Goes through each slot (that contains >= 1 item already), and makes sure the image and numbers are consistent with its assigned INventory ITem.
        {
            inventoryItemSlots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlots[i].UpdateSlot(stash[i]);
        }
    }

    public void AddItem(ItemData _item) //used to be AddItem()
    {
        if (_item.itemType == ItemType.Equipment)
        {
            AddToInventory(_item);
        }

        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);

        
        }

            UpdateSlotUI(); //this line ensures that itemSlot images and itemAmount (TMP) values are accurate


    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }

        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem inventoryValue))
        {
            if (inventoryValue.stackSize <= 1)
            {
                inventoryItemSlots[inventory.Count - 1].SetDefaultSlotImage(); //Wady's trying to get creative, will likely backfire
                inventory.Remove(inventoryValue);
                inventoryDictionary.Remove(_item);
                


            }
            else
            {
                inventoryValue.RemoveStack();
            }
           
        }

        

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI();

    }







}
