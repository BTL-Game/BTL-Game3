using UnityEngine;

// Map này mới cần CreateAssetMenu để bạn tạo file trong Project
[CreateAssetMenu(fileName = "NewIceMap", menuName = "Map System/Ice Map")]
public class IceMapData : BaseMapData 
{
    [Header("--- CƠ CHẾ RIÊNG CỦA MAP BĂNG ---")]
    public float coldRate = 10f;
    public GameObject mutantSnowflakePrefab;
}