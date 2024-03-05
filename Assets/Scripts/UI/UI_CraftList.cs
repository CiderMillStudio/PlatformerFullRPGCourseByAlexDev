using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftList : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private GameObject craftSlotPrefab;

    [SerializeField] private List<ItemDataEquipment> craftEquipment;

    private void OnEnable() //added this 2/29/2024 due to bug where item names wouldn't appear on craft slot after opening it once. (first time opening the craft tab was successful, but not subsequent times)
    {
        SetupCraftList();
        SetupDefaultCraftWindow();
    }

    private void Start() //called 4 times (because there are 4 Craft List Buttons!)
    {
        transform.parent.GetChild(0).GetComponent<UI_CraftList>().SetupCraftList();
        
    }

    public void SetupCraftList()
    {

        for (int i = 0; i < craftSlotParent.childCount; i++)
        {
            Destroy(craftSlotParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < craftEquipment.Count; i++)
        {
            GameObject newSlot = Instantiate(craftSlotPrefab, craftSlotParent);
            newSlot.GetComponent<UI_CraftSlot>().SetupCraftSlot(craftEquipment[i]);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupCraftList();
        SetupDefaultCraftWindow();
        AudioManager.instance.PlaySFX(7, null);
    }

    public void SetupDefaultCraftWindow()
    {
        if (craftEquipment[0] != null)
            GetComponentInParent<UI>().craftWindow.SetupCraftWindow(craftEquipment[0]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(36, null);
    }
}
