using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb; 
    
    private float fadeOutModifier;
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    private float attackCheckRadius;

    private Transform closestEnemy;
    private bool flipped;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();   
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime; 

        if (cloneTimer < 0 )
            sr.color  = new Color(sr.color.r, sr.color.g, sr.color.b, 
                sr.color.a -  fadeOutModifier * Time.deltaTime);
        if (sr.color.a <= 0f)
            Destroy(gameObject);
    }

    public void SetUpClone(Transform _newTransform, float _cloneDuration, 
        float _fadeOutModifier, bool canAttack, 
            float _attackCheckRadius, Vector3 _offset, Transform _closestEnemy) //sets up position and other stuff
    {
        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        
        cloneTimer = _cloneDuration;
        attackCheckRadius = _attackCheckRadius;
        transform.position = _newTransform.position + _offset; 
        fadeOutModifier = _fadeOutModifier;

        closestEnemy = _closestEnemy;
        FaceClosestTarget();

        //if (PlayerManager.instance.player.facingDir < 0)
        //sr.flipX = true;
    }

    void AnimationTrigger()
    {
        cloneTimer = -0.1f;
    }

    void AnimationMiddleTrigger1()
    {
        int facingDir;
        if (flipped) //if (sr.flipX)
            facingDir = -1;
        else
            facingDir = 1;

        rb.AddForce(new Vector2(1 /*moveSpeedProxy*/ * 10 * facingDir,
            0), ForceMode2D.Impulse);
        }

    void AnimationDamageTrigger()
    {
        Collider2D[] colliders =
            Physics2D.OverlapCircleAll(attackCheck.position,
                attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }


    private void FaceClosestTarget() //So cool!
    {
        
        
        if (closestEnemy != null)
        {
            if (closestEnemy.position.x < transform.position.x)
            {
                transform.Rotate(0, 180, 0);
                flipped = true;
            }
                //sr.flipX = true;
        }
    }    
}
