using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{

    private Animator anim => GetComponent<Animator>(); // HUH!?! No need for START statement!?
    private Player player;

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

    [SerializeField] private LayerMask whatIsEnemy;
    

    
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, 
        float _maxSize, float _growSpeed, Transform _closestTarget, Player _player)
    {
        player = _player;
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
            if (closestTarget != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, closestTarget.position) < 0.2f)
                {
                    FinishCrystal();
                }
                
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
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }    

    public void growCrystalExplosion()
    {
        if (!canExplode) { return; }

        transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackHoleRadius();


        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if (colliders.Length>0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }

        
    }
}
