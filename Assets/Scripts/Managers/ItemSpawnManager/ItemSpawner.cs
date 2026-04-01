using UnityEngine;
public class ItemSpawner : MonoBehaviour
{
    [Header("Item Prefabs")]
    public GameObject snowflakePrefab;
    public GameObject mutantSnowflakePrefab;
    public GameObject flamePrefab;
    public GameObject ghostPrefab;
    public GameObject soulFlamePrefab;
    [Header("Spawn Settings")]
    public float yOffset = 40f;
    public float distanceBetweenItems = 10f;
    public float minSpawnRate = 0.2f;
    [Header("Spawn Chances (%)")]
    [Range(0f, 100f)] public float rareItemChance = 30f;
    [Range(0f, 100f)] public float mutantChance = 10f;
    private float timer = 0f;
    public bool canSpawn = true;
    public enum SpawnerMode { Ice, Ghost }
    public SpawnerMode currentMode = SpawnerMode.Ice;
    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameStarted || !canSpawn) return;
        float currentSpeed = GameManager.Instance.gameSpeed;
        float currentSpawnRate = distanceBetweenItems / currentSpeed;
        currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate);
        if (timer < currentSpawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            SpawnItem();
        }
    }
    void SpawnItem()
    {
        GameObject prefabToSpawn = null;
        float randomValue = Random.Range(0f, 100f);
        if (currentMode == SpawnerMode.Ice)
        {
            if (snowflakePrefab == null) return;
            prefabToSpawn = snowflakePrefab;
            if (randomValue <= rareItemChance)
            {
                if (flamePrefab != null) prefabToSpawn = flamePrefab;
            }
            else if (randomValue <= rareItemChance + mutantChance)
            {
                if (mutantSnowflakePrefab != null) prefabToSpawn = mutantSnowflakePrefab;
            }
        }
        else if (currentMode == SpawnerMode.Ghost)
        {
            if (ghostPrefab == null) return;
            prefabToSpawn = ghostPrefab;
            if (randomValue <= rareItemChance)
            {
                if (soulFlamePrefab != null) prefabToSpawn = soulFlamePrefab;
            }
        }
        if (prefabToSpawn == null) return;
        float randomY = Random.Range(-yOffset, yOffset);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + randomY, transform.position.z);
        GameObject newItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
        if (newItem.GetComponent<PillarMovement>() == null && newItem.GetComponent<WallMovement>() == null && newItem.GetComponent<GhostItemMovement>() == null)
        {
            newItem.AddComponent<PillarMovement>();
        }
    }
}
