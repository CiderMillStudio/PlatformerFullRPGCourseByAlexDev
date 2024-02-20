using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText; //number value of stat
    [SerializeField] private TextMeshProUGUI statNameText; // name of the stat (set to the above statName string)

    [TextArea]
    [SerializeField] private string statDescription;

    private UI ui;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statNameText != null)
        {
            statNameText.text = statName;
        }
    }

    private void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.StatOfType(statType).GetValue().ToString();

            
            
            if (statType == StatType.health)
            {
                statValueText.text = playerStats.GetMaxHealthValue().ToString();
            }
            
            else if (statType == StatType.damage)
            {
                int damageModifiedValue = playerStats.damage.GetValue() + playerStats.strength.GetValue();
                statValueText.text = damageModifiedValue.ToString();
            }

            else if (statType == StatType.critPower)
            {
                int critPowerModifiedValue = playerStats.critPower.GetValue() + playerStats.strength.GetValue();
                statValueText.text = critPowerModifiedValue.ToString();
            }

            else if (statType == StatType.evasion)
            {
                int evasionModifiedValue = playerStats.evasion.GetValue() + playerStats.agility.GetValue();
                statValueText.text = evasionModifiedValue.ToString();
            }
            
            else if (statType == StatType.critChance)
            {
                int critChanceModifiedValue = playerStats.critChance.GetValue() + playerStats.agility.GetValue();
                statValueText.text = critChanceModifiedValue.ToString();
            }

            else if (statType == StatType.magicResistance)
            {
                statValueText.text = (playerStats.magicResistance.GetValue() 
                    + 3 * playerStats.intelligence.GetValue()).ToString();
            }


        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowStatTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideStatTooltip();
    }

    private void Update()
    {
        if (ui.statTooltip.isActiveAndEnabled && ui.statTooltip.statTooltipRectTransform != null)
        {
            ui.statTooltip.statTooltipRectTransform.position = Input.mousePosition + new Vector3(0.05f * Screen.width, 0);
            
        }
    }
}
