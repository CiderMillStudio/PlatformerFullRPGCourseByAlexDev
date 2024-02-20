using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    
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
    }

    private void Start()
    {

        skillImage = GetComponent<Image>();

        skillImage.color = lockedSkillColor;

        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                Debug.Log("You cannot unlock this skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length;i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("You cannot unlock this skill");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowToolTip(skillDescription, skillName);
        /*Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;

        if (mousePosition.x > 600)
            xOffset = -350;
        else
            xOffset = 350;

        ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y);*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideToolTip();
    }
} 
