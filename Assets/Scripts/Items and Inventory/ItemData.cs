using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
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

    public string itemId;

    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR //generates a unique ID for each individual item!
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }

}
