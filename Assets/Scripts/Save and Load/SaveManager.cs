using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //USING SYSTEM.LINQ is IMPORTANT (required for the FindAllSaveManagers() function below)

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    
    [SerializeField] private string fileName;

    [SerializeField] private GameObject skillTreePanel;

    [SerializeField] private bool encryptData;

    private List<ISaveManager> saveManagers;

    private GameData gameData;

    private FileDataHandler dataHandler;

    [ContextMenu("Delete Save File")] //THIS (context menu) is accessed by right clicking thet script component in Unity!
    public void DeleteSavedData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);

        dataHandler.DeleteData();
    }
    public void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData); //Application.persistantDataPath allows users of various operating systems (mac, windows, linux), all to be able to use their default paths. Paths specific to each operating system can be found on unity docs.
        saveManagers = FindAllSaveManagers();
        
        LoadGame();
        Debug.Log("Save Manager Loaded Game.");
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        

        dataHandler.Save(gameData);
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        if (skillTreePanel != null)
            skillTreePanel?.SetActive(true); //THIS IS SO IMPORTANT!!!! Otherwise, the FindObjectsOfType line won't be able to detect the skillTree panel, because it's technically "inactive"
        
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>(); //this is very new syntax ai ai ai... What is an IEnumerable?
        
        if (skillTreePanel != null)
            skillTreePanel?.SetActive(false); //THIS IS SO IMPORTANT!!!  

        return new List<ISaveManager>(saveManagers);
    }


    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
            return true;
        return false;
    }


}

