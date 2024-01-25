using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{

    private Animator anim => GetComponent<Animator>(); // HUH!?! No need for START statement!?

    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private float maxSize = 5f;
    private float growSpeed = 5f;
    private bool canGrow;
    private bool playerEndedCrystal; //This gets set to true whenever FinishCrystal
                                     //is called ONCE (either by the timer expiring,
                                     //or if the player prematurely presses the crystal skill hotkey.

    private Transform closestTarget;
    

    
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, float _maxSize, float _growSpeed, Transform _closestTarget)
    {
        crystalExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        closestTarget = _closestTarget;

    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0 || playerEndedCrystal)
        {
            FinishCrystal();
            
        }

        if (canMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 0.2f)
            {
                FinishCrystal();
            }
        }    
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            anim.SetTrigger("Explode");
            canGrow = true;
            playerEndedCrystal = true;
            canMove = false;
        }
        else
        {
            SelfDestroy();
        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    public void SelfDestroy() => Destroy(gameObject);


    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }    

    public void growCrystalExplosion()
    {
        if (!canExplode) { return; }

        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
    }
}
