using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{

    /*When the sword collides with its target (ground, enemy, etc...), 
     * it will fly into object, it will turn its rb type to kinematic, 
     * freeze position x and y, freeze rotation z, and we'll make it a 
     * child of the object it collided with*/

    private float returnSpeed;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate; //probably won't use this much
    private bool isReturning;

    public bool isBouncing; //temporarily setting isBouncing to true and AmountOfBounce to 4 (for testing purposes)
    public int AmountOfBounce = 4; //how many times it can bounce between targets
    public List<Transform> enemyTargets;
    private int targetIndex;
    public float bounceSpeed; //speed at which sword travels between enemies

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player, float _returnSpeed)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        returnSpeed = _returnSpeed;
        canRotate = true;
        anim.SetBool("Rotate", true);
    }

    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;

    }

    private void Update()
    {
        /*        if (canRotate)
                { 
                    //transform.right = rb.velocity; //set this at the END when the sword HITS!
                }   */

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1f)
            {
                isReturning = false;
                player.DestroySword();
            }
        }

        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                AmountOfBounce--;

                if (AmountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTargets.Count)
                    targetIndex = 0;


            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;



        if (collision.GetComponent<Enemy>() != null)
        {
            isBouncing = true;
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(hit.transform);
                    }
                }
            }
        }

        StuckInto(collision);

    }

    private void StuckInto(Collider2D collision)
    {
        if (canRotate)
            transform.right = rb.velocity;
        canRotate = false;
        rb.isKinematic = true;
        cd.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;


        if (isBouncing && enemyTargets.Count > 0)
            return;


        anim.SetBool("Rotate", false);
        transform.parent = collision.transform;


    }
}
