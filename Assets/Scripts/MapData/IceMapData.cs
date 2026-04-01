using UnityEngine;
[CreateAssetMenu(fileName = "NewIceMap", menuName = "Map System/Ice Map")]
public class IceMapData : BaseMapData 
{
    [Header("--- ICE MAP SPECIFIC MECHANICS ---")]
    public GameObject snowflakePrefab;       
    public GameObject mutantSnowflakePrefab; 
    public GameObject firePrefab;            
    public float coldRate = 2f;
}
