using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    protected PlayerStats playerStats;
    protected virtual void Start()
    {
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if (collision.GetComponent<Enemy>() != null)
        {

            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            if (enemyStats != null)
            {
                playerStats.DoMagicalDamage(enemyStats);
            }

        }

    }

}
