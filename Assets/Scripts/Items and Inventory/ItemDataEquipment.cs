using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Equipment Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData //thus, ItemDataEquipment is also a S.O., like ItemData itself
{
    public EquipmentType equipmentType;

}
