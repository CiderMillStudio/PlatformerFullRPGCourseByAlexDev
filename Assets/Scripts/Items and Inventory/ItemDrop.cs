using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{

    [SerializeField] private int maxAmountOfItemsToDrop;

    [Tooltip("the lower this value, the less likely a drop will occur")]
    [Range(0,1)]
    [SerializeField] private float dropChanceModifier;
    [SerializeField] private ItemData[] possibleDrops;
    [SerializeField] private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;



    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrops.Length; i++)
        {
            if (Random.Range(0,100) <= possibleDrops[i].dropChance)
            {
                dropList.Add(possibleDrops[i]);
            }
        }

        for (int i = 0; i < maxAmountOfItemsToDrop && dropList.Count >= 1; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count - 1)];

            if (Random.value < dropChanceModifier)
            {
            dropList.Remove(randomItem);
            DropItem(randomItem);
            }

        }
    }


    protected void DropItem(ItemData _itemData) //public because this needs to be called by the enemy
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 6), Random.Range(12, 15));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
