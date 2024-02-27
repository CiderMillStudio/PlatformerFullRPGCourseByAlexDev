using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private RectTransform activateCheckpointButtonRect; //ui_element rect transform

    [SerializeField] private RectTransform checkpointCanvasRect; //canvas rect transform

    [SerializeField] private float buttonYOffset;

    [SerializeField] LayerMask playerLayerMask;

    public string checkpointId;

    public bool checkpointActivated;
    private bool playerCanActivateCheckpoint;
    private bool inExitCoroutine;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (activateCheckpointButtonRect.gameObject.activeSelf)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 checkpointCandlabraScreenPos = new Vector2
            (
                (viewportPosition.x * checkpointCanvasRect.sizeDelta.x) - (checkpointCanvasRect.sizeDelta.x * 0.5f),      //x coord
                (viewportPosition.y * checkpointCanvasRect.sizeDelta.y) - (checkpointCanvasRect.sizeDelta.y * 0.5f)       //y coord
            );

            activateCheckpointButtonRect.anchoredPosition = checkpointCandlabraScreenPos + new Vector2 (0, buttonYOffset);
        }

        if (playerCanActivateCheckpoint && Input.GetKeyDown(KeyCode.T))
        {
            ActivateCheckpoint();
        }

    }

    [ContextMenu("Generate Checkpoint Id")]
    private void GenerateId()
    {
        checkpointId = System.Guid.NewGuid().ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !checkpointActivated && !inExitCoroutine)
        {
            checkpointCanvasRect.gameObject.SetActive(true);
            activateCheckpointButtonRect.GetComponent<UI_CheckpointActivationButton>().FadeIn();
            playerCanActivateCheckpoint = true;

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !inExitCoroutine && checkpointCanvasRect.gameObject.activeSelf && 
                !PlayerManager.instance.player.stats.isDead)
        {
            playerCanActivateCheckpoint = false;
            
            StartCoroutine(ExitingCheckpointCoroutine());

        }
    }

   
    public void ActivateCheckpoint()
    {
        checkpointActivated = true;
        anim.SetBool("active", true);
        playerCanActivateCheckpoint = false;
        
        if (checkpointCanvasRect.gameObject.activeSelf)
            checkpointCanvasRect.gameObject.SetActive(false);
    }


    private IEnumerator ExitingCheckpointCoroutine()
    {
        activateCheckpointButtonRect.GetComponent<UI_CheckpointActivationButton>().FadeOut();
        inExitCoroutine = true;


        yield return new WaitForSeconds(2f);
        
        inExitCoroutine = false;

        if (IsPlayerDetected())
        {
            activateCheckpointButtonRect.GetComponent<UI_CheckpointActivationButton>().FadeIn();
            playerCanActivateCheckpoint = true;
        }
        
        else
        {
            checkpointCanvasRect.gameObject.SetActive(false);
        }

    }

    private RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(transform.position + new Vector3(3, -2, 0), Vector2.left, 6, playerLayerMask);







}
