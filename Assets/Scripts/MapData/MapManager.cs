using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    [Header("Current Map Config")]
    public BaseMapData currentMap;

    [Header("System References")]
    [Tooltip("Gắn Parallax backgrounds từ trong Scene")]
    public ParallaxBackground farBackground;
    public ParallaxBackground midBackground;
    public ParallaxBackground nearBackground;
    
    [Tooltip("Gắn Main Pillar Spawner")]
    public PillarSpawner pillarSpawner;
    
    [Tooltip("Gắn AudioSource chứa nhạc nền của game")]
    public AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("MapManager: Bắt đầu Start() trên MapManager.");
        if (currentMap != null)
        {
            ApplyMap(currentMap);
        }
        else
        {
            Debug.LogWarning("MapManager: Ô 'Current Map' trong Inspector chưa được gắn MapData!");
        }
    }

    // Hàm public được gọi để load toàn bộ dữ liệu của Map
    public void ApplyMap(BaseMapData data)
    {
        if (data == null) {
            Debug.LogError("MapManager: Data truyền vào hàm ApplyMap bị NULL!");
            return;
        }
        currentMap = data;
        Debug.Log($"MapManager: Đang bắt đầu Apply Map: {data.mapName} (Tên asset: {data.name})");

        // 1. Áp dụng âm thanh
        if (musicSource != null)
        {
            if (data.backgroundMusic != null)
            {
                musicSource.clip = data.backgroundMusic;
                musicSource.Play();
                Debug.Log($"MapManager: Đã phát background music -> {data.backgroundMusic.name}");
            }
            else
            {
                Debug.LogWarning("MapManager: File MapData hiện tại chưa có nhạc (backgroundMusic bị trống).");
            }
        }
        else
        {
            Debug.LogError("MapManager: Chưa gắn 'Music Source' vào Inspector của MapManager!");
        }
        
        // 2. Chuyển đổi Background Textures
        if (farBackground != null) farBackground.ChangeTexture(data.farBG);
        else Debug.LogError("MapManager: Chưa gắn 'Far Background' vào Inspector của MapManager!");

        if (midBackground != null) midBackground.ChangeTexture(data.midBG);
        else Debug.LogError("MapManager: Chưa gắn 'Mid Background' vào Inspector của MapManager!");

        if (nearBackground != null) nearBackground.ChangeTexture(data.nearBG);
        else Debug.LogError("MapManager: Chưa gắn 'Near Background' vào Inspector của MapManager!");

        Debug.Log("MapManager: Đã gửi lệnh đổi Texture cho các Parallax Backgrounds.");

        // 3. Đổi loại cột (Pillar)
        if (pillarSpawner != null)
        {
            if (data.pillarPrefab != null)
            {
                pillarSpawner.SetPillarPrefab(data.pillarPrefab);
                Debug.Log($"MapManager: Đã truyền Pillar Prefab ({data.pillarPrefab.name}) cho PillarSpawner.");
            }
            else
            {
                Debug.LogWarning("MapManager: File MapData hiện tại chưa có Pillar Prefab!");
            }
        }
        else
        {
            Debug.LogError("MapManager: Chưa gắn 'Pillar Spawner' vào Inspector của MapManager!");
        }

        // 4. Các yếu tố đặc thù (VD: IceMapData, VolcanicMapData)
        if (data is IceMapData iceData)
        {
            Debug.Log($"Applied Ice Map! Cold Rate is now: {iceData.coldRate}");
            // Reset Boss về null khi ở map băng
            if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
                Debug.Log("Đã tắt Boss vì đây là Ice Map.");
            }
        }
        else if (data is VolcanicMapData volData)
        {
            if (GameManager.Instance != null && volData.mapBossPrefab != null)
            {
                GameManager.Instance.bossPrefab = volData.mapBossPrefab;
                GameManager.Instance.timeBetweenBossPhases = volData.timeBetweenBossPhases;
                Debug.Log($"Đã kích hoạt Boss cho Volcanic Map: {volData.mapBossPrefab.name}");
            }
        }
        else
        {
            // Các map thường khác cũng không có Boss
            if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
            }
        }
    }
}
