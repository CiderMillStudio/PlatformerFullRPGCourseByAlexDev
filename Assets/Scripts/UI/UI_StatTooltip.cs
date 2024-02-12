using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statDescription;

    [HideInInspector]
    public RectTransform statTooltipRectTransform;

    private void Start()
    {
        statTooltipRectTransform = GetComponent<RectTransform>();
    }

    public void ShowStatTooltip(string _text)
    {

        gameObject.SetActive(true);
        statDescription.text = _text;
    }

    public void HideStatTooltip()
    {
        
        statDescription.text = "";
        gameObject.SetActive(false);
    }
}
