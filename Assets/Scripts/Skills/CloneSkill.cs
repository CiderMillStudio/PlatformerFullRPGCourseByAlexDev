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


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetUpClone(_clonePosition, 
            cloneDuration, fadeOutModifier, canAttack, 
                PlayerManager.instance.player.attackCheckRadius, _offset);
    }

    
}
