using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{
    public Slider slider;
    public string parameter;

    

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;


    public void SliderValue(float _value)
    {

        /*if (parameter == "BGM")
        {
            GameManager.instance.currentBgmVolume = _value * multiplier;
        }
        else if (parameter == "SFX")
        {
            GameManager.instance.currentSfxVolume = _value * multiplier;
        }
        else
            Debug.Log("oops!! Not BGM nor SFX?");*/


        audioMixer.SetFloat(parameter, _value * multiplier);

    }


}
