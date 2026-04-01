using UnityEngine;
using System.Collections.Generic;
public class PillarItemSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    [Range(0f, 1f)]
    public float globalSpawnChance = 0.5f;
    public List<CollectibleData> lootTable;
    void Start()
    {
        TrySpawnItem();
    }
    public void TrySpawnItem()
    {
        if (Random.value > globalSpawnChance) return;
        int totalWeight = 0;
        foreach (var item in lootTable) totalWeight += item.spawnWeight;
        int randomNumber = Random.Range(0, totalWeight);
        int cursor = 0;
        foreach (var item in lootTable)
        {
            cursor += item.spawnWeight;
            if (randomNumber < cursor)
            {
                Instantiate(item.prefab, spawnPoint.position, Quaternion.identity, transform);
                break;
            }
        }
    }
}
