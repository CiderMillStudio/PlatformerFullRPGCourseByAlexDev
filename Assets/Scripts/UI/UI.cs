using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject[] UiPanels;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleUiMenu();
        }
    }
}
