using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;

    public void ShowToolTip(string _skillDescription, string _skillName)
    {
        
        gameObject.SetActive(true);
        
        if (skillDescription != null)
            skillDescription.text = _skillDescription;
        else
            skillDescription.text = "";

        skillName.text = _skillName;

       
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
