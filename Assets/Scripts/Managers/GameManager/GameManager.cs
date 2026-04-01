using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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
    

    void Awake()
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

    void Update()
    {
        if (isGameStarted)
        {
            if (gameSpeed < maxGameSpeed)
            {
                gameSpeed += speedMultiplier * Time.deltaTime;
            }

            if (!IsBossPhaseActive && !IsPortalPhaseActive)
            {
                if (bossPrefab != null) currentBossTimer += Time.deltaTime;
                if (portalPrefab != null) currentPortalTimer += Time.deltaTime;

                if (bossPrefab != null && currentBossTimer >= timeBetweenBossPhases)
                {
                    currentBossTimer = 0f;
                    StartCoroutine(RunBossPhase());
                }
                else if (portalPrefab != null && currentPortalTimer >= timeBetweenPortalPhases)
                {
                    currentPortalTimer = 0f;
                    StartCoroutine(RunPortalPhase());
                }
            }
        }
    }

    IEnumerator RunPortalPhase()
    {
        IsPortalPhaseActive = true;
        SetSpawnersActive(false);

        while (Object.FindFirstObjectByType<PillarMovement>() != null)
        {
            yield return null;
        }
        
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

        IsPortalPhaseActive = false;
        SetSpawnersActive(true);
    }

    IEnumerator RunBossPhase()
    {
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

        SpawnBoss();

        while (IsBossPhaseActive)
        {
            yield return null;
        }

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
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);
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

        WallMovement[] walls = Object.FindObjectsByType<WallMovement>(FindObjectsSortMode.None);
        foreach (WallMovement wall in walls)
        {
            wall.canMove = false;
        }

        if (pillarSpawner == null)
        {
            pillarSpawner = Object.FindFirstObjectByType<PillarSpawner>();
        }
        if (pillarSpawner != null)
        {
            pillarSpawner.canSpawn = false;
        }

        WallController wallController = Object.FindFirstObjectByType<WallController>();
        if (wallController != null)
        {
            wallController.StopSpawning();
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
