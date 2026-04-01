using UnityEngine;
public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }
    [Header("Current Map Config")]
    public BaseMapData currentMap;
    [Header("System References")]
    public ParallaxBackground farBackground;
    public ParallaxBackground midBackground;
    public ParallaxBackground nearBackground;
    public PillarSpawner pillarSpawner;
    public ItemSpawner itemSpawner;
    public WallController wallController;
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
        if (currentMap != null)
        {
            ApplyMap(currentMap);
        }
        else
        {
        }
    }
    public void ApplyMap(BaseMapData data)
    {
        if (data == null) {
            return;
        }
        currentMap = data;
        if (data.backgroundMusic != null)
        {
            if (MusicManager.instance != null)
            {
                if (musicSource != null) musicSource.Stop();
                MusicManager.instance.ChangeMusicWithFade(data.backgroundMusic);
            }
            else if (musicSource != null)
            {
                musicSource.clip = data.backgroundMusic;
                musicSource.Play();
            }
        }
        else
        {
        }
        if (farBackground != null) {
            farBackground.ChangeTexture(data.farBG);
        } 
        if (midBackground != null) {
            midBackground.ChangeTexture(data.midBG);
        }
        if (nearBackground != null) {
            nearBackground.ChangeTexture(data.nearBG);
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
            if (pillarSpawner != null) pillarSpawner.gameObject.SetActive(false);
            if (itemSpawner != null) 
            {
                itemSpawner.gameObject.SetActive(true);
                itemSpawner.currentMode = ItemSpawner.SpawnerMode.Ice;
                itemSpawner.snowflakePrefab = iceData.snowflakePrefab;
                itemSpawner.mutantSnowflakePrefab = iceData.mutantSnowflakePrefab;
                itemSpawner.flamePrefab = iceData.firePrefab;
            }
            if (wallController != null) 
            {
                wallController.gameObject.SetActive(false);
            }
            if (ColdSystem.Instance != null)
            {
                ColdSystem.Instance.SetActive(true, iceData.coldRate);
            }
            if (SanitySystem.Instance != null)
            {
                SanitySystem.Instance.SetActive(false);
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
            }
        }
        else if (data is VolcanicMapData volData)
        {
            if (pillarSpawner != null) pillarSpawner.gameObject.SetActive(true);
            if (itemSpawner != null) itemSpawner.gameObject.SetActive(false);
            if (wallController != null) 
            {
                wallController.gameObject.SetActive(false);
            }
            ClearGhostMapObjects();
            if (ColdSystem.Instance != null)
            {
                ColdSystem.Instance.SetActive(false);
            }
            if (SanitySystem.Instance != null)
            {
                SanitySystem.Instance.SetActive(false);
            }
            if (GameManager.Instance != null && volData.mapBossPrefab != null)
            {
                GameManager.Instance.bossPrefab = volData.mapBossPrefab;
                GameManager.Instance.timeBetweenBossPhases = volData.timeBetweenBossPhases;
            }
            else if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
            }
        }
        else if (data is GhostMapData ghostData)
        {
            if (pillarSpawner != null) pillarSpawner.gameObject.SetActive(false);
            if (itemSpawner != null) 
            {
                itemSpawner.gameObject.SetActive(true);
                itemSpawner.currentMode = ItemSpawner.SpawnerMode.Ghost;
                itemSpawner.ghostPrefab = ghostData.ghostPrefab;
                itemSpawner.soulFlamePrefab = ghostData.soulflamePrefab;
            }
            if (wallController != null) 
            {
                wallController.gameObject.SetActive(true);
                wallController.normalWall = ghostData.normalWallPrefab;
                wallController.ghostWall = ghostData.ghostWallPrefab;
            }
            if (ColdSystem.Instance != null)
            {
                ColdSystem.Instance.SetActive(false);
            }
            if (SanitySystem.Instance != null)
            {
                SanitySystem.Instance.SetActive(true, ghostData.sanityDecreaseRate);
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
            }
        }
        else
        {
            if (pillarSpawner != null) pillarSpawner.gameObject.SetActive(true);
            if (itemSpawner != null) itemSpawner.gameObject.SetActive(false);
            if (wallController != null) 
            {
                wallController.gameObject.SetActive(false);
            }
            if (ColdSystem.Instance != null)
            {
                ColdSystem.Instance.SetActive(false);
            }
            if (SanitySystem.Instance != null)
            {
                SanitySystem.Instance.SetActive(false);
            }
            if (GameManager.Instance != null)
            {
                GameManager.Instance.bossPrefab = null;
            }
        }
    }
    private void ClearGhostMapObjects()
    {
        WallMovement[] walls = FindObjectsByType<WallMovement>(FindObjectsSortMode.None);
        foreach (var wall in walls)
        {
            Destroy(wall.gameObject);
        }
        GhostItemMovement[] ghosts = FindObjectsByType<GhostItemMovement>(FindObjectsSortMode.None);
        foreach (var ghost in ghosts)
        {
            Destroy(ghost.gameObject);
        }
        CollectiblesManager[] collectibles = FindObjectsByType<CollectiblesManager>(FindObjectsSortMode.None);
        foreach (var col in collectibles)
        {
            if (col.type == CollectiblesManager.ItemType.SoulFlame || col.type == CollectiblesManager.ItemType.Ghost)
            {
                Destroy(col.gameObject);
            }
        }
    }
}
