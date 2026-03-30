using UnityEngine;

[CreateAssetMenu(fileName = "NewIceMap", menuName = "Map System/Ice Map")]
public class IceMapData : BaseMapData 
{
    [Header("--- CƠ CHẾ RIÊNG CỦA MAP BĂNG ---")]
    public GameObject snowflakePrefab;       
    public GameObject mutantSnowflakePrefab; 
    public GameObject firePrefab;            

    public float coldRate = 2f;
}