using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{

    [Header("Clone Prefab")]
    [SerializeField] private float cloneDuration;
    [SerializeField] private float attackMultiplier;
    [Tooltip("the higher this value, the faster the clone will fade out once " + 
        "[Clone Duration] number of seconds has passed")]
    [SerializeField] private float fadeOutModifier = 1f;
    
    [SerializeField] private GameObject clonePrefab;

    [Space]
    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot cloneAttackUnlockButton;
    [SerializeField] private float cloneAttackMultiplier;
    [SerializeField] private bool canAttack;

    [Space]
    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot aggressiveCloneUnlockButton;
    [SerializeField] private float aggressiveCloneAttackMultiplier;
    public bool canApplyOnHitEffect;


    [Space]
    [Header("Multiple Clone")]
    [SerializeField] private UI_SkillTreeSlot multipleCloneUnlockButton;
    [SerializeField] private float multipleCloneAttackMultiplier;
    [SerializeField] private bool canDuplicateClone;
    [Tooltip("Percent chance of the clone duplicating itself into another clone on a successful hit")]
    [SerializeField] private float chanceToDuplicateClone = 25f;

    [Space]
    [Header("Crystal made instead of clone")]
    [SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
    public bool CrystalInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultipleClone);
        crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    #region Unlock Region

    protected override void CheckUnlock()
    {
        UnlockCloneAttack();
        UnlockAggressiveClone();
        UnlockCrystalInstead();
        UnlockMultipleClone();
    }

    private void UnlockCloneAttack()
    {
        if (cloneAttackUnlockButton.unlocked)
        {
            canAttack = true;
            attackMultiplier = cloneAttackMultiplier;
        }
    }

    private void UnlockAggressiveClone()
    {
        if (aggressiveCloneUnlockButton.unlocked)
        {
            canApplyOnHitEffect = true;
            attackMultiplier = aggressiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultipleClone()
    {
        if(multipleCloneUnlockButton.unlocked)
        {
            attackMultiplier = multipleCloneAttackMultiplier;
            canDuplicateClone = true;
        }
    }

    private void UnlockCrystalInstead()
    {
        if (crystalInsteadUnlockButton.unlocked)
        {
            CrystalInsteadOfClone = true;
        }
    }


    #endregion


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (CrystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        
        newClone.GetComponent<CloneSkillController>().SetUpClone(_clonePosition, 
            cloneDuration, fadeOutModifier, canAttack, 
                PlayerManager.instance.player.attackCheckRadius, _offset, FindClosestEnemy(newClone.transform), 
                canDuplicateClone, chanceToDuplicateClone, player, attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform _enemyTransform)
    {
            StartCoroutine(CloneDelayCoroutine(_enemyTransform, new Vector3(2 * player.facingDir, 0, 0)));
    }


    private IEnumerator CloneDelayCoroutine(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }

    
}
