using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameStarted = false;
    [Header("Difficulty Settings")]
    public float gameSpeed = 5f;
    public float maxGameSpeed = 15f;
    public float speedMultiplier = 0.2f;

    [Header("Score Settings")]
    public int score = 0;

    [Header("Boss Phase Settings")]
    public float timeBetweenBossPhases = 60f;
    public PillarSpawner pillarSpawner;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    [Header("Portal Phase Settings")]
    public float timeBetweenPortalPhases = 90f;
    public GameObject portalPrefab;
    public Transform portalSpawnPoint;

    private float currentBossTimer = 0f;
    private float currentPortalTimer = 0f;
    public bool IsBossPhaseActive { get; private set; }
    public bool IsPortalPhaseActive { get; private set; }
    
    private float initialGameSpeed;

    void Awake()
    {
        Instance = this;
        initialGameSpeed = gameSpeed;
    }

    public void ResetGameSpeed()
    {
        gameSpeed = initialGameSpeed;
        Debug.Log("[GameManager] Tốc độ game đã được reset về: " + gameSpeed);
    }

    void Update()
    {
        if (gameSpeed < maxGameSpeed)
        {
            gameSpeed += speedMultiplier * Time.deltaTime;
        }

        if (isGameStarted && !IsBossPhaseActive && !IsPortalPhaseActive)
        {
            // Tăng timer nếu đang ở trạng thái bình thường
            if (bossPrefab != null) currentBossTimer += Time.deltaTime;
            if (portalPrefab != null) currentPortalTimer += Time.deltaTime;

            // Kiểm tra Boss Phase trước
            if (currentBossTimer >= timeBetweenBossPhases && bossPrefab != null)
            {
                currentBossTimer = 0f;
                // Sang Boss Phase: Timer của Portal sẽ dừng đếm do IsBossPhaseActive = true
                StartCoroutine(RunBossPhase());
            }
            // Nếu chưa gọi Boss thì kiểm tra Portal Phase
            else if (currentPortalTimer >= timeBetweenPortalPhases && portalPrefab != null)
            {
                currentPortalTimer = 0f;
                // Sang Portal Phase: Timer của Boss sẽ dừng đếm do IsPortalPhaseActive = true
                StartCoroutine(RunPortalPhase());
            }
        }
    }

    IEnumerator RunPortalPhase()
    {
        Debug.Log("[GameManager] Bắt đầu RunPortalPhase! Ẩn các spawners.");
        IsPortalPhaseActive = true;
        SetSpawnersActive(false);

        while (Object.FindFirstObjectByType<PillarMovement>() != null)
        {
            yield return null;
        }
        
        Debug.Log("[GameManager] Mọi chướng ngại vật đã qua, chuẩn bị spawn Portal.");
        Vector3 spawnPos = portalSpawnPoint != null ? portalSpawnPoint.position : new Vector3(80f, 0f, 0f);
        GameObject portalInstance = Instantiate(portalPrefab, spawnPos, Quaternion.identity);

        if (portalInstance.GetComponent<PillarMovement>() == null)
        {
            portalInstance.AddComponent<PillarMovement>();
        }

        while (portalInstance != null)
        {
            yield return null;
        }

        Debug.Log("[GameManager] Portal đã bị hủy/đi qua, kết thúc Portal phase và bật lại spawner.");
        IsPortalPhaseActive = false;
        SetSpawnersActive(true);
    }

    IEnumerator RunBossPhase()
    {
        Debug.Log("[GameManager] Bắt đầu RunBossPhase! Ẩn các spawners.");
        IsBossPhaseActive = true;
        SetSpawnersActive(false);

        // Kích hoạt rung lắc màn hình và phát âm thanh cảnh báo
        if (BossCameraShake.Instance != null)
        {
            BossCameraShake.Instance.TriggerBossWarning();
        }

        while (Object.FindFirstObjectByType<PillarMovement>() != null)
        {
            yield return null;
        }

        Debug.Log("[GameManager] Spawn Boss.");
        SpawnBoss();

        while (IsBossPhaseActive)
        {
            yield return null;
        }

        Debug.Log("[GameManager] Boss bị tiêu diệt, kết thúc Boss phase và bật lại spawner.");
        SetSpawnersActive(true);
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {

            CompleteBossPhase();
            return;
        }

        Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : new Vector3(110f, 12f, 0f);
        GameObject bossInstance = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }

    void SetSpawnersActive(bool canSpawn)
    {
        if (pillarSpawner == null)
        {
            pillarSpawner = Object.FindFirstObjectByType<PillarSpawner>();
        }

        if (pillarSpawner != null)
        {
            pillarSpawner.canSpawn = canSpawn;
        }

        ItemSpawner itemSpawner = Object.FindFirstObjectByType<ItemSpawner>();
        if (itemSpawner != null)
        {
            itemSpawner.canSpawn = canSpawn;
        }
    }

    public void CompleteBossPhase()
    {
        IsBossPhaseActive = false;
    }

    public void AddScore(int amount)
    {
        score += amount;

    }

    public void SpeedUp(float extraSpeed, float duration)
    {
        StartCoroutine(SpeedUpRoutine(extraSpeed, duration));
    }

    private IEnumerator SpeedUpRoutine(float extraSpeed, float duration)
    {
        gameSpeed += extraSpeed;

        yield return new WaitForSeconds(duration);
        gameSpeed -= extraSpeed;

    }

    public void TriggerGameOver()
    {
        isGameStarted = false;
        CompleteBossPhase();

        ParallaxBackground[] parallax = Object.FindObjectsByType<ParallaxBackground>(FindObjectsSortMode.None);
        foreach (ParallaxBackground bg in parallax)
        {
            bg.canRoll = false;
        }

        PillarMovement[] pillars = Object.FindObjectsByType<PillarMovement>(FindObjectsSortMode.None);
        foreach (PillarMovement pillar in pillars)
        {
            pillar.canMove = false;
        }

        if (pillarSpawner == null)
        {
            pillarSpawner = Object.FindFirstObjectByType<PillarSpawner>();
        }
        if (pillarSpawner != null)
        {
            pillarSpawner.canSpawn = false;
        }

        BossManager boss = Object.FindFirstObjectByType<BossManager>();
        if (boss != null)
        {
            boss.FreezeBossForGameOver();
        }

        StartCoroutine(ShowGameOverUIRoutine());
    }

    private IEnumerator ShowGameOverUIRoutine()
    {
        yield return new WaitForSeconds(3f); 

        GameOverManager gameOverManager = Object.FindFirstObjectByType<GameOverManager>(FindObjectsInactive.Include);
        if (gameOverManager != null)
        {
            gameOverManager.Setup(score);
        }
    }
}
