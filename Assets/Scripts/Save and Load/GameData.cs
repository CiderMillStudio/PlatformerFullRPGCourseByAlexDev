using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //IMPORTANT!
public class GameData  //DOES NOT inherit from Monobehavior
{
    public int currency;

    public SerializableDictionary<string, int> inventory;

    public SerializableDictionary<string, bool> skillTree;

    public List<string> equipmentId;

    public GameData()
    {
        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
    }
}
