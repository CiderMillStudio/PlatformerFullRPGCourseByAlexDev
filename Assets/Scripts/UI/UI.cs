using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterPanel;
    [SerializeField] private GameObject skilltreePanel;
    [SerializeField] private GameObject craftPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject inGameUi;
    [SerializeField] public GameObject fadeScreen;
    [SerializeField] public GameObject deathText;

    [SerializeField] private GameObject[] UiMenuPanels;

    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_SkillTooltip skillTooltip;

    public UI_CraftWindow craftWindow;

    private void Awake()
    {
        SwitchTo(skilltreePanel);
    }

    private void Start()
    {
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
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < UiMenuPanels.Length; i++)
        {
            UiMenuPanels[i].gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void ToggleUiMenu()
    {
        foreach (GameObject _menu in UiMenuPanels)
        {
            if (_menu.activeInHierarchy)
            {
                _menu.SetActive(false);
                CheckForInGameUI();
                return;
            }
        }

        inGameUi.SetActive(false);
        characterPanel.SetActive(true);
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
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

    private void CheckForInGameUI()
    {
        for (int i = 0; i < UiMenuPanels.Length; i++)
        {
            inGameUi.gameObject.SetActive(false);

            if (UiMenuPanels[i].gameObject.activeSelf)
            {
                return;
            }
        }

        ItemDataEquipment currentFlask = Inventory.instance.GetEquipment(EquipmentType.Flask);
        inGameUi.GetComponent<UI_InGamePanel>().SetFlaskImage(currentFlask);


        SwitchTo(inGameUi);
    }

    public void SwitchOnEndScreen()
    {
        deathText.SetActive(true);
        fadeScreen.GetComponent<UI_FadeScreen>().FadeOut(3f);
        deathText.GetComponent<Animator>().SetTrigger("Death");
    }
}
