using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarToggleButton : MonoBehaviour
{
    public void TogglePlayerHealthBar()
    {
        if (Time.time < 0.25f)
            return;

        if (!this.GetComponent<Toggle>().isOn)
        {
            GameManager.instance.enabledPlayerHealthBar = false;
        }
        else
        {
            GameManager.instance.enabledPlayerHealthBar = true;
        }

        Debug.Log("HEalth Bar toggle: " + GameManager.instance.enabledPlayerHealthBar);

    }

    public void SetStartingToggleValue()
    {
        
        this.GetComponent<Toggle>().isOn = GameManager.instance.enabledPlayerHealthBar;
    }
}
