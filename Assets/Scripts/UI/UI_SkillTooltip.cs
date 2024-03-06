using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultNameFontSize = 42;
    public void ShowToolTip(string _skillDescription, string _skillName, int _price)
    {
        
        gameObject.SetActive(true);
        
        if (skillDescription != null)
            skillDescription.text = _skillDescription;
        else
            skillDescription.text = "";

        skillName.text = _skillName;
        skillCost.text = "Cost: $" + _price.ToString();

        AdjustPosition();
        AdjustFontSize(skillName);
       
    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }

    public override void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePosition.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePosition.y > yLimit)
            newYOffset = Mathf.RoundToInt(0.33f * yOffset);
        else
            newYOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + newXOffset, mousePosition.y + newYOffset);
    }

}
