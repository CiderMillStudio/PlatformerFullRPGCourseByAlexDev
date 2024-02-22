using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill //NEW SCRIPT!
{

    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space]    
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    BlackholeSkillController currentBlackhole;

    [Header("Blackhole Ability")]
    [SerializeField] private UI_SkillTreeSlot blackholeUnlockButton;
    public bool blackholeUnlocked {  get; private set; }

    
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    private void UnlockBlackhole()
    {
        if (blackholeUnlockButton.unlocked)
        {
            blackholeUnlocked = true;
        }
    }

    public override void UseSkill() 
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position - new Vector3(0,1), Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackholeDuration);


    }

    protected override void Start()
    {
        base.Start();

        blackholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        
        if (!currentBlackhole)
            return false;
        
        if (currentBlackhole.playerCanExitState == true)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackHoleRadius()
    {
        return (maxSize / 2);
    }
}
