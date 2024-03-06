using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI ui;

    [SerializeField] private int skillCost;
    [SerializeField] private string skillName;

    [TextArea]
    [SerializeField] private string skillDescription;

    [SerializeField] private Color lockedSkillColor;
    
    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }


    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {

        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColor;

        if (unlocked)
        {
            skillImage.color = Color.white;
        }

    }

    public void UnlockSkillSlot()
    {
        
        

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("You cannot unlock this skill");
                AudioManager.instance.PlaySFX(118, null);
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length;i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("You cannot unlock this skill");
                AudioManager.instance.PlaySFX(118, null);
                return;
            }
        }
        
        if (PlayerManager.instance.HaveEnoughMoney(skillCost) == false)
        {
            AudioManager.instance.PlaySFX(118, null);
            return;
        }

        ui.SetSkillTreeCurrencyText();
        unlocked = true;
        AudioManager.instance.PlaySFX(99, null);
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowToolTip(skillDescription, skillName, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            unlocked = value; //But this is not enough!!! Gotta go into SKILL and INDIVIDUAL SKILLS and set THEIR booleans to TRUE in their START() methods!!!!!
        }   //Spoiler alert: Also had to bring Save Manager script to act BEFORE the default scripts (see SCript Ordering)
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
        {
            _data.skillTree.Add(skillName, unlocked);
        }
    }
} 
