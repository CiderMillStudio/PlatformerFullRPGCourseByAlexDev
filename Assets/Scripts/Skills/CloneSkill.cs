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
                PlayerManager.instance.player.attackCheckRadius, _offset, FindClosestEnemy(newClone.transform), 
                canDuplicateClone, chanceToDuplicateClone, player);
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
