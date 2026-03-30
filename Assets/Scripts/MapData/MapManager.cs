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
    [Tooltip("Gắn Item Spawner (chỉ dùng cho map có item như Ice Map)")]
    public ItemSpawner itemSpawner;

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
        Debug.Log("[MapManager] Khởi tạo Game, Load Map hiện tại nếu có.");
        if (currentMap != null)
        {
            ApplyMap(currentMap);
        }
        else
        {
            Debug.LogWarning("[MapManager] Chưa có Map mặc định, hãy set currentMap trên inspector!");
        }
    }

    public void ApplyMap(BaseMapData data)
    {
        if (data == null) {
            Debug.LogWarning("[MapManager] ApplyMap được gọi nhưng map data bị NULL!");
            return;
        }
        
        Debug.Log($"[MapManager] Đang áp dụng map mới: {data.mapName ?? data.name}. Boss phase type: {data.GetType().Name}");
        currentMap = data;

        if (data.backgroundMusic != null)
        {
            if (MusicManager.instance != null)
            {
                if (musicSource != null) musicSource.Stop();
                MusicManager.instance.ChangeMusicWithFade(data.backgroundMusic);
                Debug.Log($"[MapManager] Đổi nhạc nền qua MusicManager: {data.backgroundMusic.name}");
            }
            else if (musicSource != null)
            {
                musicSource.clip = data.backgroundMusic;
                musicSource.Play();
                Debug.Log($"[MapManager] Đổi nhạc nền trực tiếp tại musicSource: {data.backgroundMusic.name}");
            }
        }
        else
        {
            Debug.Log("[MapManager] Map này không có nhạc nền.");
        }

        Debug.Log($"[MapManager] Cập nhật hình nền Parallax:");
        if (farBackground != null) {
            farBackground.ChangeTexture(data.farBG);
            Debug.Log($"[MapManager] - FarBG: {(data.farBG != null ? data.farBG.name : "NULL")}");
        } 

        if (midBackground != null) {
            midBackground.ChangeTexture(data.midBG);
            Debug.Log($"[MapManager] - MidBG: {(data.midBG != null ? data.midBG.name : "NULL")}");
        }

        if (nearBackground != null) {
            nearBackground.ChangeTexture(data.nearBG);
            Debug.Log($"[MapManager] - NearBG: {(data.nearBG != null ? data.nearBG.name : "NULL")}");
        }

        if (pillarSpawner != null)
        {
            pillarSpawner.SetPillarPrefab(data.pillarPrefab);
            if (data.pillarPrefab != null)
            {

            }
            else
            {

            }
        }
        else
        {

        }

        if (data is IceMapData iceData)
        {
            Debug.Log("[MapManager] Áp dụng cài đặt cho ICE MAP (Spawner Items thay vì Pillars).");
            if (pillarSpawner != null) pillarSpawner.gameObject.SetActive(false);
            if (itemSpawner != null) itemSpawner.gameObject.SetActive(true);

            if (ColdSystem.Instance != null)
            {
                ColdSystem.Instance.SetActive(true, iceData.coldRate);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
                Debug.Log("[MapManager] Xóa Boss prefab vì Ice Map không có boss.");
            }
            itemSpawner.snowflakePrefab = iceData.snowflakePrefab;
            itemSpawner.mutantSnowflakePrefab = iceData.mutantSnowflakePrefab;
            itemSpawner.flamePrefab = iceData.firePrefab;
        }
        else if (data is VolcanicMapData volData)
        {
            Debug.Log("[MapManager] Áp dụng cài đặt cho VOLCANIC MAP (Kích hoạt Pillars, chuẩn bị Boss).");
            if (pillarSpawner != null) pillarSpawner.gameObject.SetActive(true);
            if (itemSpawner != null) itemSpawner.gameObject.SetActive(false);

            if (ColdSystem.Instance != null)
            {
                ColdSystem.Instance.SetActive(false);
            }

            if (GameManager.Instance != null && volData.mapBossPrefab != null)
            {
                GameManager.Instance.bossPrefab = volData.mapBossPrefab;
                GameManager.Instance.timeBetweenBossPhases = volData.timeBetweenBossPhases;
                Debug.Log($"[MapManager] Cập nhật Boss prefab: {volData.mapBossPrefab.name}");
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
            }
        }
        else
        {
            Debug.Log("[MapManager] Áp dụng cài đặt cho DEFAULT MAP (Chỉ có Pillars).");
            if (pillarSpawner != null) pillarSpawner.gameObject.SetActive(true);
            if (itemSpawner != null) itemSpawner.gameObject.SetActive(false);

            if (ColdSystem.Instance != null)
            {
                ColdSystem.Instance.SetActive(false);
            }

            if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
            }
        }
    }
}
