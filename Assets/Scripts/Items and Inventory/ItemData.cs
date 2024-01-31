using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item/Moosey")] //(try "Data/Item/MooseyMoose")
public class ItemData : ScriptableObject //INHERIT FROM S.O.!!
{
    public string itemName;
    public Sprite icon;
}
