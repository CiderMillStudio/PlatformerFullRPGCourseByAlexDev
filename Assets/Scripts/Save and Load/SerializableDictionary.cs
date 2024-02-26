using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //IMPORTANT
public class SerializableDictionary<TKey, TValue>/* <-- important!!!*/  : Dictionary<TKey, TValue> , ISerializationCallbackReceiver //IMPORTANT!!! 
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize() //iffy??? not sure how/why this works.
    {
        keys.Clear();
        values.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
        {
            Debug.Log("keys.Count is not equal it values.Count");
        }

        for (int i = 0; i < keys.Count; i++) 
        {
            this.Add(keys[i], values[i]);
        }
    }

}
