using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/ItemEffect/Thunder Strike")]

public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    public override void ExecuteEffect(Transform _respawnPosition)
    {
        //delete base!

        GameObject newThunderStrike = Instantiate(thunderStrikePrefab, _respawnPosition.position, Quaternion.identity);
        AudioManager.instance.PlaySFX(Random.Range(90, 93), newThunderStrike.transform);
        //TODO: create a Setup function for newThunderStrike (i.e. we need to make a Thunder Strike Controller script!!!) (actually not anymore!)
        
        Destroy(newThunderStrike, 0.5f);

    }
}
