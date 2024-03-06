using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private RectTransform activateCheckpointButtonRect; //ui_element rect transform

    [SerializeField] private RectTransform checkpointCanvasRect; //canvas rect transform

    [SerializeField] private float buttonYOffset;

    [SerializeField] LayerMask playerLayerMask;

    [SerializeField] ParticleSystem healReadyParticleSystemLoop;
    [SerializeField] ParticleSystem healCompleteParticleSystem;

    public string checkpointId;

    public bool checkpointActivated;
    private bool playerCanActivateCheckpoint;
    private bool inExitCoroutine;


    private bool canHealPlayer;
    [SerializeField] private float healPlayerCooldown;
    private float healPlayerCooldownTimer;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        canHealPlayer = true;
        healPlayerCooldownTimer = 0;
        healReadyParticleSystemLoop.Play();

    }

    private void Update()
    {

        if (!canHealPlayer)
        {
            healPlayerCooldownTimer -= Time.deltaTime;

        }

        if (!canHealPlayer && healPlayerCooldownTimer <= 0)
        {
            canHealPlayer = true;
            healReadyParticleSystemLoop.Play();
        }

        if (activateCheckpointButtonRect.gameObject.activeSelf)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
            Vector2 checkpointCandlabraScreenPos = new Vector2
            (
                (viewportPosition.x * checkpointCanvasRect.sizeDelta.x) - (checkpointCanvasRect.sizeDelta.x * 0.5f),      //x coord
                (viewportPosition.y * checkpointCanvasRect.sizeDelta.y) - (checkpointCanvasRect.sizeDelta.y * 0.5f)       //y coord
            );

            activateCheckpointButtonRect.anchoredPosition = checkpointCandlabraScreenPos + new Vector2(0, buttonYOffset);
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
        bool playerAtMaxHealth = false;

        if (PlayerManager.instance.player.stats.currentHealth == PlayerManager.instance.player.stats.GetMaxHealthValue())
            playerAtMaxHealth = true;

        if (collision.GetComponent<Player>() != null && !checkpointActivated && !inExitCoroutine)
        {
            checkpointCanvasRect.gameObject.SetActive(true);
            activateCheckpointButtonRect.GetComponent<UI_CheckpointActivationButton>().FadeIn();
            playerCanActivateCheckpoint = true;


        }

        if (collision.GetComponent<Player>() && checkpointActivated && canHealPlayer && Time.time > 0.5f && !playerAtMaxHealth)
        {
            PlayerManager.instance.player.stats.IncreaseHealthBy(PlayerManager.instance.player.stats.GetMaxHealthValue());
            canHealPlayer = false;
            healPlayerCooldownTimer = healPlayerCooldown;
            healReadyParticleSystemLoop.Stop();
            healCompleteParticleSystem.Play();
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
        if (Time.time > 0.5f)
        {
            PlayerManager.instance.player.stats.IncreaseHealthBy(PlayerManager.instance.player.stats.GetMaxHealthValue());
            canHealPlayer = false;
            healPlayerCooldownTimer = healPlayerCooldown;
            healReadyParticleSystemLoop.Stop();
            healCompleteParticleSystem.Play();

        }

        AudioManager.instance.PlaySFX(5, this.transform);
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
