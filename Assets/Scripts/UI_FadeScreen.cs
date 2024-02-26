using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        anim.SetBool("FadeOut", false);
    }

    public void FadeOut(float _delay)
    {
        StartCoroutine(FadeOutWithDelay(_delay));
    }

    private IEnumerator FadeOutWithDelay(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        anim.SetBool("FadeOut", true);
        if (PlayerManager.instance != null)
        {
            if (PlayerManager.instance.player.stats.isDead)
            {
                yield return new WaitForSeconds(4f);
                SaveManager.instance.SaveGame();
                SceneManager.LoadScene("StartMenu");
            }
        }
        
    }


}
