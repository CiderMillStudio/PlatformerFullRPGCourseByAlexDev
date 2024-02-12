using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;

    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private int defaultFontSize = 32;

    [HideInInspector]
    public RectTransform itemTooltipRectTransform;

    private void Start()
    {
        itemTooltipRectTransform = GetComponent<RectTransform>();
    }
    public void ShowToolTip(ItemDataEquipment item)
    {
        if (item == null)
        {
            return; 
        }

        gameObject.SetActive(true);

        itemNameText.text = item.itemName;
        itemTypeText.text = item.equipmentType.ToString();
        itemDescription.text = item.GetDescription();

        if (itemNameText.text.Length > 12)
            itemNameText.fontSize = defaultFontSize * 0.7f;
        else
            itemNameText.fontSize = defaultFontSize;


    }


    public void HideToolTip()
    {
        itemNameText.fontSize = defaultFontSize;
        gameObject.SetActive(false);
    }
}
