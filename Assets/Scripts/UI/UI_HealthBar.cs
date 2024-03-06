using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //!!!! !!! ! ! !!! NEEDED TO ADD THIS MANUALLY!!!

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;

    private RectTransform myRectTransform;//Because the EntityStatus_UI has a Rect Transform instead of a normal Transform.

    private Slider slider;

    private CharacterStats myStats;

    private bool flipUiAdded = false;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        myRectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        
        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void Update()
    {
        //UpdateHealthUI(); //by adding an EVENT (UpdateHealthUI) to CharacterStats.cs, we no longer need to
        //call this line in update (we save processing power!)
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }

    private void FlipUI()
    {
        myRectTransform.Rotate(0, 180, 0);
    }


    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
    //it's good practice to unsubscribe from events if something is happening (we could have an OnEnable function,
    //but it's weird because OnEnable happens after start)

}
