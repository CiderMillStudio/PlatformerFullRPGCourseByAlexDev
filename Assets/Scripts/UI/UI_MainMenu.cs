using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; //IMPORTANT!

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";

    [SerializeField] private Button continueButton;

    [SerializeField] private UI_FadeScreen fadeScreen;

    [SerializeField] private float fadeDuration = 3f;


    private void Start()
    {
        if (SaveManager.instance.HasSavedData() == false)
        {
            continueButton.gameObject.SetActive(false);
            fadeScreen.FadeIn();
        }

        AudioManager.instance.FadeInBgmVolume(6, 10f);
    }

    public void ContinueGame()
    {
        
        StartCoroutine(ContinueGameWithDelayForFade(fadeDuration));
    }

    public void NewGame()
    {

        StartCoroutine(NewGameWithDelayForFade(fadeDuration));
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game");
        AudioManager.instance.FadeOutBgmVolume(6);
        Application.Quit();
    }


    private IEnumerator ContinueGameWithDelayForFade(float _delay)
    {
        fadeScreen.FadeOut(0f);
        AudioManager.instance.FadeOutBgmVolume(6);

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator NewGameWithDelayForFade(float _delay)
    {
        fadeScreen.FadeOut(0f);
        AudioManager.instance.FadeOutBgmVolume(6);

        yield return new WaitForSeconds(_delay);

        SaveManager.instance.DeleteSavedData();
        SceneManager.LoadScene(sceneName);
    }
}
