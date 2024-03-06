using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = PlayerManager.instance.player;
        if (collision.GetComponent<Entity>() != null)
        {
            collision.GetComponent<CharacterStats>().KillEntity(true);
            collision.GetComponent<Entity>().DieFromPlayerScript();
            GameManager.instance.SetLostCurrencyXYCoords(player.transform.position.x, player.transform.position.y);
        }
    }
}
