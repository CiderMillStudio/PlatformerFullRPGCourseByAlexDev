using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] UiPanels;

    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;

    private void Start()
    {
        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public void ToggleUiMenu()
    {
        foreach (GameObject _menu in UiPanels)
        {
            if (_menu.activeInHierarchy)
            {
                _menu.SetActive(false);
                return;
            }
        }

        UiPanels[0].SetActive(true);   
    }

    public void ToggleToSpecificUIMenuWithShortcut(GameObject _specificMenu)
    {
        foreach (GameObject menu in UiPanels)
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
            ToggleToSpecificUIMenuWithShortcut(UiPanels[0]); //UiPanels[0] is Character Panel
        }

        else if (Input.GetKeyUp(KeyCode.V))
        {
            ToggleToSpecificUIMenuWithShortcut(UiPanels[1]); //UiPanels[0] is SkillTree Panel
        }

        else if (Input.GetKeyUp(KeyCode.B))
        {
            ToggleToSpecificUIMenuWithShortcut(UiPanels[2]); //UiPanels[2] is Craft Panel
        }

        else if (Input.GetKeyUp(KeyCode.N))
        {
            ToggleToSpecificUIMenuWithShortcut(UiPanels[3]); //UiPanels[3] is Options Panel
        }



    }
}
