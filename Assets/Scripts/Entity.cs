using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    #region Collisions
    [Header("CollisionInfo")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    public Transform attackCheck;
    public float attackCheckRadius;

    #endregion
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }

    public SpriteRenderer sr { get; private set; }

    public CharacterStats stats { get; private set; } 


    #endregion

    #region Knockback
    [Header("KnockBack info")]
    public Vector2 knockbackDirection;
    protected bool isKnocked;
    [SerializeField] protected float knockbackDuration = 0.2f;

    #endregion

    public int facingDir { get; private set; } = 1;

    protected bool facingRight = true;

    public CapsuleCollider2D cd { get; private set; }




    public System.Action onFlipped; //NEW!!!!!





    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        fx = GetComponentInChildren<EntityFX>(); 
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>(); 
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        //enemies and player have different movespeed, so we must
        //override this function in Enemy (perhaps in subtypes of enemy) scripts and Player script.
    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact() => StartCoroutine("HitKnockback");


    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        rb.velocity = new Vector2 (knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }


    #region Collision Methods
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position,
        Vector3.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, 
        Vector3.right * facingDir, wallCheckDistance, whatIsGround);
    #endregion

    #region Flip Methods
    public virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFlipped != null) //NEW !!!! delete this line to see a coocoo error (player doesn't have a healthbarUI, so nothing to flip!)
            onFlipped(); //NEW!!
    }

    public virtual void FlipController(float _x)
    {
        if (_x < 0 && facingRight)
            Flip();

        else if (_x > 0 && !facingRight)
            Flip();

    }
    #endregion

    #region Player Velocity Methods
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity); //now we can Flip the sprite in multiple states!
    }

    public virtual void SetZeroVelocity()
    {
        if (isKnocked)
            return;
            
        rb.velocity = new Vector2 (0, rb.velocity.y);

    }


    #endregion

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x,
            groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * facingDir,
            wallCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }



    public virtual void Die()
    {
        //overridden in player and in enemy scripts
        if (GetComponent<Player>() == null)
            Invoke("SelfDestroyAfterDeath", 10f); //after ten seconds, the enemy will have fallen down very
                                                    //low in the map. (given we're using a super mario style death).
                                                    //this only happens to enemies or NPCs, not the player.
    }

    private void SelfDestroyAfterDeath()
    {
        Destroy(gameObject);
    }


}
