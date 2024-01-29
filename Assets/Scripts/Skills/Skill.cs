using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    /*This will be the grand daddy skill master class which encodes functionality that 
     all skills will have in common. For instance, a cooldown timer, etc... */

    [SerializeField] protected float cooldown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }


    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        //Debug.Log("Skill is on cooldown");
        return false;
    }


    public virtual void UseSkill()
    {
        //do some skill-specific things
        //all skills will override this function to perform their unique functions
    }


    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {
        

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 100);
        float closestDistance = Mathf.Infinity;

        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        return closestEnemy;
    }
}
