using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //THIS IS IMPORTANT, makes Stat VISIBLE in INSPECTOR!

public class Stat //DOES NOT INHERIT FROM MONOBEHAVIOR!!!!
{
    [SerializeField] private int baseValue; //base value of a stat

    public List<int> modifiers; //stores list of modifiers, usually from ailments or from equipment;

    public int GetValue() 
    {
        int finalValue = baseValue;

        if (modifiers.Count >= 0)
        {
            foreach (int modifier in modifiers)
            {
                finalValue += modifier;
            }

        }

        return finalValue;
    }

    public void SetDefaultValue (int _value) 
    {
        baseValue = _value;
    }

    public void AddModifier(int _modifier) //useful when equipping weapons, armor, etc..
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier) //useful when un-equipping weapons, armor, etc..
    {
        modifiers.Remove(_modifier);
    }


}

