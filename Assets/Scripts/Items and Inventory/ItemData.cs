using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public enum ItemType
{
    Material,
    Equipment

}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")] //(try "Data/Item/MooseyMoose")
public class ItemData : ScriptableObject //INHERIT FROM S.O.!!
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDescription()
    {
        return "";
    }

}
