using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{

    private CharacterStats targetStats;
    [SerializeField] private float speed;

    private int damage;

    private Animator anim;
    private bool triggered = false;

    private float thunderFlipTimer;


    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        thunderFlipTimer = 0.07f;
    }

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetStats = _targetStats;
    }

    void Update()
    {
        if (!targetStats)
            return;
        
        thunderFlipTimer -= Time.deltaTime;
        
        
        if (triggered) { return; }

        

        

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position; //orients the lightening bolt!

        if (thunderFlipTimer <= 0 && !triggered)
        {
            float thunderTimeBetweenFlips = Random.Range(0.03f, 0.2f);
            thunderFlipTimer = thunderTimeBetweenFlips;

            FlipThunderBolt();
        }

        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            triggered = true;

            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            anim.transform.localPosition = new Vector3(0, 0.4f, 0); //slightly elevates the lightning bolt so it
                                                                    //doesn't end up going through the ground
            Invoke("DamageAndSelfDestroy", 0.2f);

            anim.SetTrigger("Hit");
            

            
        }
        
    }

    private void DamageAndSelfDestroy()
    {

        targetStats.TakeDamage(damage, this.transform);
            targetStats.ApplyShock(true);
            Destroy(gameObject, 0.4f);
    }

    private void FlipThunderBolt()
    {
        if (anim.GetComponent<SpriteRenderer>().flipX)
            anim.GetComponent<SpriteRenderer>().flipX = false;
        else
            anim.GetComponent<SpriteRenderer>().flipX = true;

        //Debug.Log("Thunder Flipped!");
    }
}
