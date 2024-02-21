using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("Restore Health on Parry")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f,1f)]
    [SerializeField] private float restoreHealthPercentage;
    public bool restoreUnlocked { get; private set; }


    [Header("Parry with Mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
    public bool parryWithMirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }

    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked)
        {
            restoreUnlocked = true;
        }
    }

    private void UnlockParryWithMirage()
    {
        if (parryWithMirageUnlockButton.unlocked)
        {
            parryWithMirageUnlocked = true;
        }
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
        {
            SkillManager.instance.clone.CreateCloneWithDelay(_respawnTransform);
        }
    }

}
