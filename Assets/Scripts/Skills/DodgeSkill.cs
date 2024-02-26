using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
    [SerializeField] private int evasionAmount = 10; //as a percentage!
    public bool dodgeUnlocked {  get; private set; }

    [Header("Dodge Mirage")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeMirageButton;
    public bool dodgeMirageUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();

        unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockDodgeMirageButton.GetComponent<Button>().onClick.AddListener(UnlockDodgeMirage);
    }

    protected override void CheckUnlock() //WOW 2/24/2024
    {
        UnlockDodge();
        UnlockDodgeMirage();
    }

    private void UnlockDodge()
    {
        if (unlockDodgeButton.unlocked && !dodgeUnlocked) //by adding !dodgeUnlocked, we prevent players from increasing their evasion stat to infinity by repeat-clicking the dodgeUnlockButton.
        {
            player.stats.evasion.AddModifier(evasionAmount);
            dodgeUnlocked = true;
            Inventory.instance.UpdateStatsUI();
        }
    }

    private void UnlockDodgeMirage()
    {
        if (unlockDodgeMirageButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void CreateMirageOnDodge(Transform _enemyTransform)
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.instance.clone.CreateClone(_enemyTransform, new Vector3(1.5f * -_enemyTransform.GetComponent<Enemy>().facingDir, 0, 0));
        }
    }
}
