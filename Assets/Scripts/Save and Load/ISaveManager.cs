using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveManager //Don't forget to change type to INTERFACE (from CLASS)
{
    void LoadData(GameData _data);
    void SaveData(ref GameData _data);
}
