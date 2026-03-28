using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameStarted = false;
    [Header("Difficulty Settings")]
    public float gameSpeed = 5f;
    public float maxGameSpeed = 20f;
    public float speedMultiplier = 0.2f;

    [Header("Score Settings")]
    public int score = 0;

    [Header("Boss Phase Settings")]
    public float timeBetweenBossPhases = 30f;
    public PillarSpawner pillarSpawner;
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    private Coroutine bossLoopCoroutine;
    public bool IsBossPhaseActive { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (gameSpeed < maxGameSpeed)
        {
            gameSpeed += speedMultiplier * Time.deltaTime;
        }

        if (isGameStarted && bossLoopCoroutine == null)
        {
            bossLoopCoroutine = StartCoroutine(BossPhaseLoop());
        }
    }

    IEnumerator BossPhaseLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenBossPhases);

            if (!isGameStarted || IsBossPhaseActive)
            {
                continue;
            }

            yield return StartCoroutine(RunBossPhase());
        }
    }

    IEnumerator RunBossPhase()
    {
        IsBossPhaseActive = true;
        SetPillarSpawning(false);

        // Wait for all existing pillars to be cleared before spawning the boss.
        while (Object.FindFirstObjectByType<PillarMovement>() != null)
        {
            yield return null;
        }

        SpawnBoss();

        while (IsBossPhaseActive)
        {
            yield return null;
        }

        SetPillarSpawning(true);
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogWarning("Boss prefab is missing in GameManager.");
            CompleteBossPhase();
            return;
        }

        Vector3 spawnPos = bossSpawnPoint != null ? bossSpawnPoint.position : new Vector3(110f, 12f, 0f);
        GameObject bossInstance = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
    }

    void SetPillarSpawning(bool canSpawn)
    {
        if (pillarSpawner == null)
        {
            pillarSpawner = Object.FindFirstObjectByType<PillarSpawner>();
        }

        if (pillarSpawner != null)
        {
            pillarSpawner.canSpawn = canSpawn;
        }
    }

    public void CompleteBossPhase()
    {
        IsBossPhaseActive = false;
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    public void SpeedUp(float extraSpeed, float duration)
    {
        StartCoroutine(SpeedUpRoutine(extraSpeed, duration));
    }

    private IEnumerator SpeedUpRoutine(float extraSpeed, float duration)
    {
        gameSpeed += extraSpeed;
        Debug.Log("Speed increased by " + extraSpeed + "!");
        yield return new WaitForSeconds(duration);
        gameSpeed -= extraSpeed;
        Debug.Log("Speed returned to normal.");
    }
}
