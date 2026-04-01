using UnityEngine;
[CreateAssetMenu(fileName = "NewGhostMap", menuName = "Map System/Ghost Map")]
public class GhostMapData : BaseMapData 
{
    [Header("--- GHOST MAP SPECIFIC MECHANICS ---")]
    public GameObject normalWallPrefab;       
    public GameObject ghostWallPrefab; 
    public GameObject ghostPrefab;
    public GameObject soulflamePrefab;
    public float sanityDecreaseRate = 2f;
}
