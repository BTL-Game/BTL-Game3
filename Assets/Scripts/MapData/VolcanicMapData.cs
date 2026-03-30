using UnityEngine;

[CreateAssetMenu(fileName = "NewVolcanicMap", menuName = "Map System/Volcanic Map")]
public class VolcanicMapData : BaseMapData 
{

    [Header("Boss Settings")]
    public GameObject mapBossPrefab;
    public float timeBetweenBossPhases = 30f;
}