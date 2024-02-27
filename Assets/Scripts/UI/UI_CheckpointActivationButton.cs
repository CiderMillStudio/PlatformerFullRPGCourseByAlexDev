using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_CheckpointActivationButton : MonoBehaviour
{
    [SerializeField] private Image blackImage;
    [SerializeField] private Image frameImage;

    [Range(0f, 1f)]
    [SerializeField] private float defaultBlackImageAlphaValue = 0.66f;
    [Range(0f, 1f)]
    [SerializeField] private float defaultFrameImageAlphaValue = 1f;

    [SerializeField] private TextMeshProUGUI[] buttonTextMeshPros;
    [SerializeField] private float fadeInOutRate = 100;

    private bool canFadeIn = true;

    public void FadeIn()
    {
        if (canFadeIn)
        {
            StopAllCoroutines();
            StartCoroutine(FadeInCoroutine(1/fadeInOutRate));
        }
        
    }
    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutCoroutine(1 / fadeInOutRate));

    }


    private IEnumerator FadeInCoroutine(float _fadeInOutRate)
    {
        
        while (blackImage.color.a < defaultBlackImageAlphaValue || frameImage.color.a < defaultFrameImageAlphaValue)
        {
            if (blackImage.color.a <  defaultBlackImageAlphaValue)
                blackImage.color += new Color(0, 0, 0, 0.01f);

            frameImage.color += new Color(0, 0, 0, 0.01f);
        
            foreach (TextMeshProUGUI tmp in buttonTextMeshPros)
            {
                tmp.color += new Color(0, 0, 0, 0.01f);
            }
            
            yield return new WaitForSeconds(_fadeInOutRate);
        }


    }

    private IEnumerator FadeOutCoroutine(float _fadeInOutRate)
    {

        while (blackImage.color.a > 0 || frameImage.color.a > 0)
        {
            if (blackImage.color.a > 0)
                blackImage.color -= new Color(0, 0, 0, 0.01f);

            frameImage.color -= new Color(0, 0, 0, 0.01f);

            foreach (TextMeshProUGUI tmp in buttonTextMeshPros)
            {
                tmp.color -= new Color(0, 0, 0, 0.01f);
            }

            yield return new WaitForSeconds(_fadeInOutRate);
        }


    }

    public void DisableCheckpointButton()
    {
        canFadeIn = false;
        FadeOut();
    }

}
