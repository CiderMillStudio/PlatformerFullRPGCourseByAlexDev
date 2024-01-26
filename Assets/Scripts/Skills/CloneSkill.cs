using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{

    [Header("Clone Prefab")]
    [SerializeField] private float cloneDuration;
    [Tooltip("the higher this value, the faster the clone will fade out once " + 
        "[Clone Duration] number of seconds has passed")]
    [SerializeField] private float fadeOutModifier = 1f;
    
    [SerializeField] private GameObject clonePrefab;

    [Space]
    [SerializeField] private bool canAttack;


    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashFinish;
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    [Space]
    [SerializeField] private bool canDuplicateClone;
    [Tooltip("Percent chance of the clone duplicating itself into another clone on a successful hit")]
    [SerializeField] private float chanceToDuplicateClone = 25f;

    [Header("Crystal made instead of clone")]
    public bool CrystalInsteadOfClone;


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
                PlayerManager.instance.player.attackCheckRadius, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicateClone);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashFinish()
    {
        if (createCloneOnDashFinish)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
        {
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(2 * player.facingDir, 0, 0)));
        }
    }


    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector3 _offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(_transform, _offset);
    }

    
}
