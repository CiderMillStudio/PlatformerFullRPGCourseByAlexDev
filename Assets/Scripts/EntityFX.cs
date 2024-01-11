using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private Material originalMat;
    [SerializeField] private float flashFxTime = 0.17f;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashFxTime);

        sr.material = originalMat;

       
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
            sr.color = Color.white;
        else
            sr.color = Color.red;

    }

    private void StopRedColorBlink()
    {
        CancelInvoke("RedColorBlink");

        sr.color = Color.white;
    }






}
