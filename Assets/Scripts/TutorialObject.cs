using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    [SerializeField] private RectTransform activateTutorialButtonRect; //ui_element rect transform

    [SerializeField] private RectTransform tutorialCanvasRect; //canvas rect transform

    [SerializeField] private float buttonYOffset;

    [SerializeField] LayerMask playerLayerMask;

    [SerializeField] ParticleSystem tutorialReadyParticleSystemLoop;
    [SerializeField] ParticleSystem tutorialCompleteParticleSystem;

    public bool tutorialActivated;
    private bool playerCanActivateTutorial;
    private bool inExitCoroutine;

    [SerializeField] private GameObject tutorialWindow;

   

    private void Awake()
    {
       
        tutorialWindow.SetActive(false);
        if (tutorialCanvasRect.gameObject.activeSelf)
            tutorialCanvasRect.gameObject.SetActive(false);
            
        tutorialReadyParticleSystemLoop.Play();

    }

    private void Update()
    {

        if (activateTutorialButtonRect.gameObject.activeSelf)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 tutorialCandlabraScreenPos = new Vector2
            (
                (viewportPosition.x * tutorialCanvasRect.sizeDelta.x) - (tutorialCanvasRect.sizeDelta.x * 0.5f),      //x coord
                (viewportPosition.y * tutorialCanvasRect.sizeDelta.y) - (tutorialCanvasRect.sizeDelta.y * 0.5f)       //y coord
            );

            activateTutorialButtonRect.anchoredPosition = tutorialCandlabraScreenPos + new Vector2(0, buttonYOffset);
        }

        if (playerCanActivateTutorial && Input.GetKeyDown(KeyCode.T))
        {
            ActivateTutorial();
        }

        if (tutorialCanvasRect.gameObject.activeSelf && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.N)))
            tutorialCanvasRect.gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Player>() != null && !inExitCoroutine)
        {
            tutorialCanvasRect.gameObject.SetActive(true);
            activateTutorialButtonRect.GetComponent<UI_CheckpointActivationButton>().FadeIn();
            playerCanActivateTutorial = true;


        }

/*        if (collision.GetComponent<Player>() && tutorialActivated && Time.time > 0.5f)
        {
            
            tutorialReadyParticleSystemLoop.Stop();
        }*/

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !inExitCoroutine && tutorialCanvasRect.gameObject.activeSelf &&
                !PlayerManager.instance.player.stats.isDead)
        {
            playerCanActivateTutorial = false;

            StartCoroutine(ExitingTutorialCoroutine());

        }

        tutorialWindow.SetActive(false);
    }


    public void ActivateTutorial()
    {

        AudioManager.instance.PlaySFX(123, this.transform);
        tutorialActivated = true;

        if (!tutorialWindow.activeInHierarchy)
            tutorialWindow.SetActive(true);
        else
            tutorialWindow.SetActive(false);

        //if (tutorialCanvasRect.gameObject.activeSelf)
            //tutorialCanvasRect.gameObject.SetActive(false);


    }


    private IEnumerator ExitingTutorialCoroutine()
    {
        activateTutorialButtonRect.GetComponent<UI_CheckpointActivationButton>().FadeOut();
        inExitCoroutine = true;


        yield return new WaitForSeconds(2f);

        inExitCoroutine = false;

        if (IsPlayerDetected())
        {
            activateTutorialButtonRect.GetComponent<UI_CheckpointActivationButton>().FadeIn();
            playerCanActivateTutorial = true;
        }

        else
        {
            tutorialCanvasRect.gameObject.SetActive(false);
        }

    }

    private RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(transform.position + new Vector3(3, -2, 0), Vector2.left, 6, playerLayerMask);

}
