using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;



public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance; // PUBLIC STATIC

    #region Inventory Lists and Dictionaries
    public List<ItemData> startingItems;

    public List<InventoryItem> inventory; //was inventoryItems
    public Dictionary<ItemData, InventoryItem> inventoryDictionary; //Dictionary = Paired {Key + Value} data types

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    public List<InventoryItem> equipment;
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    #endregion

    #region Inventory Slot Types
    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent; 

    private UI_ItemSlot[] inventoryItemSlots;
    private UI_ItemSlot[] stashItemSlots; 
    private UI_EquipmentSlot[] equipmentSlots;
    private UI_StatSlot[] statSlots;

    #endregion

    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    private float armorCooldown; 
    private float flaskCooldown;


    [Header("Database")]
    public List<InventoryItem> loadedItems;
    public List<ItemDataEquipment> loadedEquipment;

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
        statSlots = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        if (loadedEquipment.Count > 0)
        {
            foreach (ItemDataEquipment item in loadedEquipment)
            {
                EquipItem(item);
            }
        }

        if (loadedItems.Count > 0)  //if there are any loaded items (i.e. if there was a save prior to this load),                                              //              //
        {
            foreach (InventoryItem item in loadedItems)   
            {
                for (int i = 0; i < item.stackSize; i++)
                {
                    AddItem(item.data); //then we will add those items into our player's inventory!
                }
            }
            return; //Then, we'll return early so that 'starting items' won't be added at all (see below)
        }

        
        
        for (int i = 0; i < startingItems.Count; i++) //this for loop will only happen if there was not a prior save!
        {
            AddItem(startingItems[i]);
        }
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
        if (!CanAddItem())
        {
            Debug.Log("Inventory is full, cannot unequip item");
            return;
        }
        else if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            itemToRemove.RemoveModifiers();
            equipmentDictionary.Remove(itemToRemove);
            //UpdateSlotUI(); //DELETE MAYBE --> Yes definitely, was resulting in an error. Error resolved once this was commented out.
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
        for (int i = 0; i < equipmentSlots.Length; i++) //Maybe delete? this is something that wady added 2/2/2024
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

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlots.Length; i++) // update infor of stats in character UI //NEW!!!!
        {
            statSlots[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item) //used to be AddItem()
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
        {
            AddToInventory(_item);
        }

        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
            Debug.Log("Adding " +  _item.itemName + " to stash!");

        
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

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlots.Length)
        {
            Debug.Log("No More Space");
            return false;
        }
        else
            return true;
    }

    public bool CanCraft(ItemDataEquipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data, out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requiredMaterials[i].stackSize) //this line saved me many lines of code
                {
                    int difference = _requiredMaterials[i].stackSize - stashValue.stackSize;
                    Debug.Log("You need " + difference.ToString() +  " more " + _requiredMaterials[i].data.itemName + 
                        " to craft " + _itemToCraft.itemName); //e.g. "You need 3 more Iron to craft Iron Armor"

                    return false;
                }
                else
                {
                    materialsToRemove.Add(_requiredMaterials[i]); //just fixed this!
                }
            }

            else // if you have ZERO of the required materials (e.g. recipe calls for 5 stone, but you have zero!)
            {
                Debug.Log("You don't have any " + _requiredMaterials[i].data.itemName + "s! Go find some!");
                return false;
            }

        }

        for (int i = 0; i < materialsToRemove.Count; i++) 
        {
            int numberOfItemsToConsume = materialsToRemove[i].stackSize;
            for (int j = 0; j <  numberOfItemsToConsume; j++)
            {
                RemoveItem(materialsToRemove[i].data);
            }
            
        }

        AddItem(_itemToCraft);

        Debug.Log("Here's your hand-crafted " + _itemToCraft.itemName);

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;

    public ItemDataEquipment GetEquipment(EquipmentType _equipmentType)
    {
        ItemDataEquipment equippedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {

            if (item.Key.equipmentType == _equipmentType)
            {
                equippedItem = item.Key;
            }

        }

        return equippedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        //See if we can use flask
        //set up cool down for flask use
        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask on Cooldown");
        }


    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);
        if (Time.time > lastTimeUsedArmor + armorCooldown) //at the beginning of the session, armorCooldown is zero, so it will work in the beginning of the game!
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor is on Cooldown");
        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.inventory) // this line acquires the items the player had in inventory last session (in form of serializable dictionary, of which each element consists of a itemId key and a stacksize value.)
        {
            foreach (var item in GetItemDatabase()) //this line acquires ALL unique items in the whole game.
            {
                if (item != null && item.itemId == pair.Key) //now, we'll compare each unique item's itemId to the itemId (pair.key) of each item that was in our inventory during the last save.
                {
                    InventoryItem itemToLoad = new InventoryItem(item); //IF and ONLY IF those itemId's match, we will generate a new InventoryItem (of ItemData type 'item')
                    itemToLoad.stackSize = pair.Value;  //and we will assign its stacksize to the amount from the prior save (pair.Value)

                    loadedItems.Add(itemToLoad); //finally, we'll add that InventoryItem to the List of Inventory Items called 'loadedItems'
                }
            }
        }

        foreach (string loadedEquipmentId in _data.equipmentId)
        {
            foreach (var item in GetItemDatabase())
            {
                if (item != null && item.itemId == loadedEquipmentId)
                {
                    loadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)     //in inventory: 2 wooden swords, 1 stone sword
    {
        _data.inventory.Clear();  //clear serializable dictionary from previous save file
        _data.equipmentId.Clear(); //DON'T FORGET THIS!!!!

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary) //for each item currently in our inventory,
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize); // we'll add an element to GameData's recently cleared serializable dictionary, consisting of an itemId (long string name), and a stack size.
        } 


        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);

            Debug.Log("Saved " + pair.Key.itemName);
        }

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemId);
            Debug.Log("Saved " + pair.Key.itemName + " in Equipment Slot " + pair.Key.equipmentType.ToString());
        }

    }

    private List<ItemData> GetItemDatabase() //return a list of every single item (itemData) we've made)
    {
        List<ItemData> itemDatabase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] {"Assets/Data/Items"});

        foreach (string scriptableObjectName in assetNames)
        {
            var scriptableObjectPath = AssetDatabase.GUIDToAssetPath(scriptableObjectName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(scriptableObjectPath);
            itemDatabase.Add(itemData);
        }

        return itemDatabase;
    }

}
