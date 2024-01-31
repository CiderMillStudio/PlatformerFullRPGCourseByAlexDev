using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Inventory : MonoBehaviour
{
    public static Inventory instance; // PUBLIC STATIC

    //public List<ItemData> inventory = new List<ItemData>(); //was this

    public List<InventoryItem> inventoryItems;

    public Dictionary<ItemData, InventoryItem> inventoryDictionary; //Dictionary = Paired {Key + Value} data types

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent; //NEW!
    private UI_ItemSlot[] itemSlots;


    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        itemSlots = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>(); //Creates an array of all the itemSlots in the inventory UI

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) //This is just for debugging purposes, delete later.
        {
            ItemData newItem = inventoryItems[inventoryItems.Count - 1].data;

            RemoveItem(newItem);

        }    
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItems.Count; i++) //Goes through each slot (that contains >= 1 item already), and makes sure the image and numbers are consistent with its assigned INventory ITem.
        {
            itemSlots[i].UpdateSlot(inventoryItems[i]);
        }
    }

    public void AddItem(ItemData _item) 
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }

        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }

        UpdateSlotUI(); //this line ensures that itemSlot images and itemAmount (TMP) values are accurate

    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }
        }

        UpdateSlotUI();

    }







}
