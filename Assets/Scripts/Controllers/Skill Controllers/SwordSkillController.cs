using System.Collections.Generic;
using Unity.Burst.CompilerServices;
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
    private float freezeTimeDuration;  //NEWWWWWW


    [Header("Bounce Info")]
    private bool isBouncing; //temporarily setting isBouncing to true and AmountOfBounce to 4 (for testing purposes).
                                   //These values will later be controlled by "upgrades" that you can use to upgrade this skill
    private int bounceAmount; //how many times it can bounce between targets
    private List<Transform> enemyTargets;
    private int targetIndex;
    private float bounceSpeed; //speed at which sword travels between enemies

    [Header("Pierce Info")]
    [Tooltip("How many enemies should the sword pierce through before sticking to an enemy.")]
    private int pierceAmount; //How many enemies should the sword pierce through before sticking to an enemy.

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning; //"spinning" here refers to the "stopped chainsaw" state of the sword, NOT the "Rotate" animation
    private bool swordShouldStick; //this was added for the Spin Sword type, because the sword would "chainsaw through the wall"
    //private bool swordStuck; //This is to prevent resetting the transform.right direction in the StuckInto() method)


    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;



    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe() //NEW 
    {
        Destroy(gameObject);
    }

    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player, float _returnSpeed, float _freezeTimeDuration)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        canRotate = true;
        swordShouldStick = false;
        //swordStuck = false; //bad idea, got rid of it, messy messy

        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (pierceAmount <= 0)
            anim.SetBool("Rotate", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7f);
    }

    public void SetUpBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        bounceSpeed = _bounceSpeed;
        enemyTargets = new List<Transform>(); //YOU NEED TO INSTANTIATE PRIVATE ENTITIES!!! Public entities get made automatically (bias much?),
                                              //and if you don't instantiate the private ones, you'll get ERRORS!!
    }

    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        spinDuration = _spinDuration;
        maxTravelDistance = _maxTravelDistance;
        hitCooldown = _hitCooldown;
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
                player.CatchTheSword();
            }
        }

        BounceLogic();

        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning && !isReturning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance)
            {
                StopWhenSpinning();
            }

            if (wasStopped && !swordShouldStick)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);



                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }

                if (spinTimer <= 0)
                {
                    isSpinning = false;
                    isReturning = true;
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        if (!wasStopped)
            spinTimer = spinDuration;
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        
    }

    private void BounceLogic()
    {
        

        if (!isBouncing || enemyTargets.Count <= 0)
        {
            return;
        }


        transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < 0.1f)
        {
            SwordSkillDamage(enemyTargets[targetIndex].GetComponent<Enemy>());

            targetIndex++;
            bounceAmount--;

            if (bounceAmount <= 0)
            {
                isBouncing = false;
                isReturning = true;
            }

            if (targetIndex >= enemyTargets.Count)
                targetIndex = 0;


        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;
        SetupTargetsForBounce(collision);

        StuckInto(collision);

    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            //isBouncing = true; I think this will be set to FALSE by default up TOP, and then when we unlock the skill "bounce" upgrade, it will be set to true by some external function.
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
    }

    private void StuckInto(Collider2D collision)
    {
        
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }    

        if (isSpinning && collision.GetComponent<Enemy>() != null) 
        {
            StopWhenSpinning();
            return; 
        }

        if (isBouncing && enemyTargets.Count > 0)
            return;

        swordShouldStick = true;
        if (canRotate)
            transform.right = rb.velocity;
        canRotate = false;
        rb.isKinematic = true;
        cd.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;





        anim.SetBool("Rotate", false);
        transform.parent = collision.transform; //stick to object!


    }

    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);

        ItemDataEquipment equippedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

        if (equippedAmulet != null)
        {
            equippedAmulet.Effect(enemy.transform);
        }
    }
}
