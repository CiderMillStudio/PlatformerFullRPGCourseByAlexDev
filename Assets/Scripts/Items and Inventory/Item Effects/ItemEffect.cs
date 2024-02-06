using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//WE DELETED THE CREATE ASSET MENU [CreateAssetMenu(fileName = "New Item Effect", menuName = "Data/ItemEffect")]
public class ItemEffect : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _respawnTransform)
    {
        Debug.Log("BASE Item Effect Executed! (nothing happens; no effect!)");
    }
}
