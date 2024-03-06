using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HardcoreModeToggleButton : MonoBehaviour
{
    

    public void ToggleHardCoreMode()
    {
        if (Time.time < 0.25f)
            return;

        if (!this.GetComponent<Toggle>().isOn)
        {
            GameManager.instance.enabledHardCoreMode = false;
        }
        else
        {
            GameManager.instance.enabledHardCoreMode = true;
        }

        Debug.Log("hardcore mode toggle: " + GameManager.instance.enabledHardCoreMode);

    }

    public void SetStartingToggleValue()
    {
        
        this.GetComponent<Toggle>().isOn = GameManager.instance.enabledHardCoreMode;

    }

}
