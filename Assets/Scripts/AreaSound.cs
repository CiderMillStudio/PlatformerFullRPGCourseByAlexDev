using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSound : MonoBehaviour
{
    [SerializeField] private int areaSoundIndex;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {

            AudioManager.instance.FadeInSfxVolume(areaSoundIndex, 10f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {

            AudioManager.instance.FadeOutSfxVolume(areaSoundIndex, 10);
        }
    }
}
