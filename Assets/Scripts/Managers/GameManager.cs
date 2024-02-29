using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements; //IMPORTANT!!

public class GameManager : MonoBehaviour, ISaveManager //DON'T FORGET TO IMPLEMENT GAME MANAGER INTERFACE!
{

    public static GameManager instance;

    [SerializeField] private UnityEngine.UI.Slider sfxSlider;
    [SerializeField] private UnityEngine.UI.Slider bgmSlider;
    [SerializeField] private AudioMixer audioMixer;


    [SerializeField] private Checkpoint[] checkpoints;

    private Transform player;

    [Header("Lost Currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyXPosition;
    [SerializeField] private float lostCurrencyYPosition;

    [Space]
    [Header("Options & Settings")]
    public float currentBgmVolume;
    public float currentSfxVolume;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
        
        checkpoints = FindObjectsOfType<Checkpoint>(); //So smarty pants, instead of needing to click and drag EVERY CHECK POINT!!
    }

    private void Start()
    {

        player = PlayerManager.instance.player.transform;
    }
    public void RestartCurrentScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {

            foreach (Checkpoint checkpoint in checkpoints)
            {

                if (pair.Key == checkpoint.checkpointId)
                {
                    if (pair.Value == true)
                    {
                        checkpoint.ActivateCheckpoint();
                    }

                }
            }
        }
    }

    private void PlacePlayerAtNearestCheckpoint(GameData _data)
    {
        if (_data.closestCheckpointId == null)
            return;


        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (checkpoint.checkpointId == _data.closestCheckpointId)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }

    private void LoadLostCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyXPosition = _data.lostCurrencyX;
        lostCurrencyYPosition = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyXPosition, lostCurrencyYPosition), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
       /* sfxSlider.value = _data.sfxVolume;
        audioMixer.SetFloat("SFX", _data.sfxVolume);

        bgmSlider.value = _data.backgroundMusicVolume;
        audioMixer.SetFloat("BGM", _data.backgroundMusicVolume);*/

        yield return new WaitForSeconds(0.1f);
        LoadCheckpoints(_data);
        PlacePlayerAtNearestCheckpoint(_data);
        LoadLostCurrency(_data);


        
        

    }

    public void SaveData(ref GameData _data)
    {
        _data.checkpoints.Clear();
        _data.closestCheckpointId = string.Empty;
        
        foreach (Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.checkpointId, checkpoint.checkpointActivated);
        }

        _data.closestCheckpointId = FindClosestCheckpoint()?.checkpointId;

        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;


/*        _data.backgroundMusicVolume = currentBgmVolume;
        _data.sfxVolume = currentSfxVolume;*/
        

    }


    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;
        Vector3 playerPos = player.position;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            
            if (checkpoint.checkpointActivated == true)
            {
                float distanceBetweenPlayerAndThisCheckpoint = Vector2.Distance(checkpoint.transform.position, playerPos);
                

                if (distanceBetweenPlayerAndThisCheckpoint < closestDistance)
                {
                    closestDistance = distanceBetweenPlayerAndThisCheckpoint;
                    closestCheckpoint = checkpoint;

                }
            }
        }

        if (closestCheckpoint == null)
            Debug.Log("No checkpoint in this scene has been activated!");

        return closestCheckpoint;

    }


    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }
}
