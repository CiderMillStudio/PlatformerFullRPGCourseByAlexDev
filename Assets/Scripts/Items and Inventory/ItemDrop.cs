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



    public virtual void GenerateDropUponDeath()
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
                DropItem(randomItem, false);
            }

        }
    }


    protected void DropItem(ItemData _itemData, bool _alwaysDropForwards) //public because this needs to be called by the enemy
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        StartCoroutine("PlayDropSFX", newDrop.transform);

        if (!_alwaysDropForwards)
        {
            Vector2 randomVelocity = new Vector2(Random.Range(-5, 6), Random.Range(12, 15));
            newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
        }

        else
        {
            float facingDir = GetComponent<Entity>().facingDir;
            Vector2 randomForwardVelocity = new Vector2(Random.Range(3, 6) * facingDir, Random.Range(7, 10));
            newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomForwardVelocity);
        }

    }

    public virtual void DropItemNoVelocity(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        StartCoroutine("PlayDropSFX", newDrop.transform);
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, Vector2.zero);
    }

    private IEnumerator PlayDropSFX(Transform _itemDropTransform)
    {
        yield return new WaitForSeconds(Random.Range(0.7f, 1.2f));

        AudioManager.instance.PlaySFX(Random.Range(56, 61), _itemDropTransform);
    }
}
