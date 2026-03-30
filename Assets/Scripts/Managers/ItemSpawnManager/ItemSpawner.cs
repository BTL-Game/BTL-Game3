using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Prefabs")]
    public GameObject snowflakePrefab;
    public GameObject mutantSnowflakePrefab;
    public GameObject flamePrefab;

    [Header("Spawn Settings")]
    public float yOffset = 40f;
    public float distanceBetweenItems = 15f;
    public float minSpawnRate = 0.5f;

    [Header("Spawn Chances (%)")]
    [Range(0f, 100f)] public float flameChance = 20f;
    [Range(0f, 100f)] public float mutantSnowflakeChance = 10f;

    private float timer = 0f;
    public bool canSpawn = true;

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

        if (snowflakePrefab == null) return;

        GameObject prefabToSpawn = snowflakePrefab; 

        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= flameChance)
        {
            if (flamePrefab != null) prefabToSpawn = flamePrefab;
        }
        else if (randomValue <= flameChance + mutantSnowflakeChance)
        {
            if (mutantSnowflakePrefab != null) prefabToSpawn = mutantSnowflakePrefab;
        }

        float randomY = Random.Range(-yOffset, yOffset);
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + randomY, transform.position.z);

        GameObject newItem = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);

        if (newItem.GetComponent<PillarMovement>() == null)
        {
            newItem.AddComponent<PillarMovement>();
        }
    }
}
