using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("Checkpoints")]
    private CheckpointParent checkpointParent;

    [Header("Death Screen")]
    public GameObject fadeScreen;
    public GameObject deathText;
    public GameObject respawnButton;
    [SerializeField] private float delayAfterDeathBeforeEndScreenAppears = 3f;

    [Space]
    [Header("UI Panels")]
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject skilltreePanel;
    [SerializeField] private GameObject craftPanel;
    [SerializeField] private GameObject optionsPanel;

    [Space]
    [SerializeField] private GameObject[] UiMenuPanels;
    [Space]
    [Header("In-Game UI")]
    [SerializeField] private GameObject inGameUi;
    [SerializeField] private GameObject playerEntityStatusAndHealthBar;

    [Space]
    [Header("Tooltips")]
    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_SkillTooltip skillTooltip;

    public UI_CraftWindow craftWindow;

    [Space]
    [Header("Volume Sliders")]
    [SerializeField] private UI_VolumeSlider[] volumeSliders;

    [Header("Toggle Settings")]
    [SerializeField] private HardcoreModeToggleButton hardCoreModeToggleButton;
    [SerializeField] private PlayerHealthBarToggleButton playerHealthModeToggleButton;

    [Space]
    [Header("Other Info")]
    [SerializeField] private TextMeshProUGUI currencyTextInSkillTree;
    private bool hasInitializedSkillTreeCurrencyText = false;


    

    private void Awake()
    {
        checkpointParent = FindObjectOfType<CheckpointParent>();
        SwitchTo(skilltreePanel);
        fadeScreen.SetActive(true);

        

    }

    private void Start()
    {
        hasInitializedSkillTreeCurrencyText = false;

        optionsPanel.SetActive(true);
        playerEntityStatusAndHealthBar.SetActive(true);
         
        hardCoreModeToggleButton.SetStartingToggleValue();
        playerHealthModeToggleButton.SetStartingToggleValue();
        optionsPanel.SetActive(false);

        SwitchTo(inGameUi);

        if (itemTooltip != null)
        {
        itemTooltip.gameObject.SetActive(false);

        }

        if (statTooltip != null)
        {
        statTooltip.gameObject.SetActive(false);
        }

        if (skillTooltip != null)
        {
            skillTooltip.gameObject.SetActive(false);

        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!hasInitializedSkillTreeCurrencyText)
            {
                skilltreePanel.SetActive(true);
                SetSkillTreeCurrencyText();
                skilltreePanel.SetActive(false);
                hasInitializedSkillTreeCurrencyText = true;
            }

            ToggleUiMenu();
        }

        else if (Input.GetKeyUp(KeyCode.C))
        {
            ToggleToSpecificUIMenuWithShortcut(characterPanel); //UiPanels[0] is Character Panel
            CheckForInGameUI();
        }

        else if (Input.GetKeyUp(KeyCode.V))
        {
            ToggleToSpecificUIMenuWithShortcut(skilltreePanel); //UiPanels[0] is SkillTree Panel
            SetSkillTreeCurrencyText();
            hasInitializedSkillTreeCurrencyText = true;
            CheckForInGameUI();
        }

        else if (Input.GetKeyUp(KeyCode.B))
        {
            ToggleToSpecificUIMenuWithShortcut(craftPanel); //UiPanels[2] is Craft Panel
            
            CheckForInGameUI();
        }

        else if (Input.GetKeyUp(KeyCode.N))
        {
            ToggleToSpecificUIMenuWithShortcut(optionsPanel); //UiPanels[3] is Options Panel
            CheckForInGameUI();
        }

    }
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < UiMenuPanels.Length; i++)
        {
            UiMenuPanels[i].gameObject.SetActive(false);
        }

        

        if (_menu != null)
        {
            _menu.SetActive(true);
            AudioManager.instance.PlaySFX(7, null);

            if (hasInitializedSkillTreeCurrencyText && _menu == skilltreePanel)
                SetSkillTreeCurrencyText();

            if (_menu != inGameUi)
            {
                GameManager.instance.PauseGame(true);
            }
            else if (_menu == inGameUi)
            {
                GameManager.instance.PauseGame(false);
            }
        }


        
    }

    public void SetSkillTreeCurrencyText()
    {

        if (PlayerManager.instance.GetCurrentCurrency() <= 0)
        {
            currencyTextInSkillTree.text = "$0";
        }
        else
            currencyTextInSkillTree.text = "$" + PlayerManager.instance.GetCurrentCurrency().ToString("#,#");
    }

    public void ToggleUiMenu()
    {
        foreach (GameObject _menu in UiMenuPanels)
        {
            if (_menu.activeInHierarchy)
            {
                _menu.SetActive(false);
                CheckForInGameUI();
                GameManager.instance.PauseGame(false);
                return;
            }
        }

        inGameUi.SetActive(false);
        characterPanel.SetActive(true);

        AudioManager.instance.PlaySFX(7, null);

        GameManager.instance.PauseGame(true);
    }

    public void ToggleToSpecificUIMenuWithShortcut(GameObject _specificMenu)
    {
        foreach (GameObject menu in UiMenuPanels)
        {
            if (menu.activeInHierarchy)
            {
                menu.SetActive(false);
                
            }
        }

        _specificMenu.SetActive(true);

        if (_specificMenu == skilltreePanel)
            SetSkillTreeCurrencyText();
            
        AudioManager.instance.PlaySFX(7, null);
    }


    private void CheckForInGameUI()
    {
        for (int i = 0; i < UiMenuPanels.Length; i++)
        {
            inGameUi.gameObject.SetActive(false);

            if (UiMenuPanels[i].gameObject.activeSelf)
            {
                GameManager.instance.PauseGame(true);
                return;
            }
        }

        ItemDataEquipment currentFlask = Inventory.instance.GetEquipment(EquipmentType.Flask);
        inGameUi.GetComponent<UI_InGamePanel>().SetFlaskImage(currentFlask);


        SwitchTo(inGameUi);
        GameManager.instance.PauseGame(false);

    }

    public void SwitchOnEndScreen()
    {
        StartCoroutine(EndScreenDelayCoroutine(delayAfterDeathBeforeEndScreenAppears));
    }

    public void RestartGameButton() => GameManager.instance.RestartCurrentScene(); 

    private IEnumerator EndScreenDelayCoroutine(float _delay)
    {
        checkpointParent.DisableAllCheckpointButtons();
        deathText.SetActive(true);

        yield return new WaitForSeconds(_delay);
        fadeScreen.GetComponent<UI_FadeScreen>().FadeOut(0);
        deathText.GetComponent<Animator>().SetTrigger("Death");

        yield return new WaitForSeconds(3f);
        respawnButton.SetActive(true);
    }

    public void LoadData(GameData _data)
    {
        foreach (UI_VolumeSlider volumeSlider in volumeSliders)
        {
            foreach (KeyValuePair<string, float> pair in _data.volumeSliders)
            {
                if (pair.Key == volumeSlider.parameter)
                {
                    volumeSlider.SliderValue(pair.Value);
                    volumeSlider.slider.value = pair.Value;
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSliders.Clear();


        foreach (UI_VolumeSlider volumeSlider in volumeSliders)
        {
            _data.volumeSliders.Add(volumeSlider.parameter, volumeSlider.slider.value);
        }
    }


    public void ExitGame()
    {
        SaveManager.instance.SaveGame();
        Application.Quit();
    }
}
