using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;


    [SerializeField] private bool cloneLeftBehindAfterTeleportation;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multistacking crystal")]
    [SerializeField] private bool canUseMultistacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float multistackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalsLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if (currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (!canMoveToEnemy)
            {
                Vector2 playerPos = player.transform.position;
                player.transform.position = currentCrystal.transform.position;
                currentCrystal.transform.position = playerPos;

                if (cloneLeftBehindAfterTeleportation)
                {
                    SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                    Destroy(currentCrystal);
                }
                else
                {
                    currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();
                }

            }

        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController currentCrystalScript = currentCrystal.GetComponent<CrystalSkillController>();

        currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed,
            maxSize, growSpeed, FindClosestEnemy(currentCrystal.transform), player);



    }

    public void CurrentCrystalChooseRandomTarget()
    {
        currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultistacks)
        {
            if (crystalsLeft.Count > 0)
            {
                if (crystalsLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);

                cooldown = 0;

                GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalsLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, 
                    moveSpeed, maxSize, growSpeed, FindClosestEnemy(newCrystal.transform), player);

                if (crystalsLeft.Count <= 0)
                {
                    
                    cooldown = multistackCooldown;
                    RefillCrystals();
                    
                }
            }

            return true;
        }
        return false;
    }

    private void RefillCrystals()
    {
        int amountToAdd = amountOfStacks - crystalsLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = multistackCooldown;
        RefillCrystals();
    }


}
