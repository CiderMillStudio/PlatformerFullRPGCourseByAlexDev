using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //IMPORTANT!
public class GameData  //DOES NOT inherit from Monobehavior
{
    public int currency;

    public SerializableDictionary<string, int> inventory;

    public SerializableDictionary<string, bool> skillTree;

    public List<string> equipmentId;
    
    public SerializableDictionary<string, bool> checkpoints;

    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;
    public bool killedByDeadZone;

    public float sfxVolume;
    public float backgroundMusicVolume;

    public SerializableDictionary<string, float> volumeSliders;

    public bool enableHardCoreMode;

    public bool playerHealthBarEnabled;
    public bool enabledPlayerHealthBar;
    

    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;
        
        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;

        sfxVolume = 0;
        backgroundMusicVolume = 0;

        volumeSliders = new SerializableDictionary<string, float>();

        enableHardCoreMode = false;
        playerHealthBarEnabled = false;
        enabledPlayerHealthBar = true;
        killedByDeadZone = false;



    }
}
