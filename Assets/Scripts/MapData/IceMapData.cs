using UnityEngine;

// Map này mới cần CreateAssetMenu để bạn tạo file trong Project
[CreateAssetMenu(fileName = "NewIceMap", menuName = "Map System/Ice Map")]
public class IceMapData : BaseMapData 
{
    [Header("--- CƠ CHẾ RIÊNG CỦA MAP BĂNG ---")]
    public GameObject snowflakePrefab;       // Bông tuyết thường (tăng lạnh ít)
    public GameObject mutantSnowflakePrefab; // Bông tuyết đột biến (đóng băng)
    public GameObject firePrefab;            // Cục lửa (giảm lạnh)

    public float coldRate = 2f;
}