using UnityEngine;

public class PillarSpawner : MonoBehaviour
{
    public GameObject pillarPrefab;
    [Header("Spawn Chance (%)")]
    [Range(0f, 100f)] 
    public float mutantChance = 5f;
    public float minLength = 5f;
    public float maxLength = 15f;
    private float timer = 0f;
    public bool canSpawn = true;
    public float distanceBetweenPillars = 15f;
    public float minSpawnRate = 0.3f;

    // Cập nhật Prefab từ MapManager
    public void SetPillarPrefab(GameObject prefab)
    {
        pillarPrefab = prefab;
        if (prefab != null)
        {
            Debug.Log($"PillarSpawner: Đã thay đổi Pillar Prefab thành công -> {prefab.name}");
        }
        else
        {
            Debug.Log("PillarSpawner: Pillar Prefab được gán vào là NULL. Sẽ không spawn pillar ở map này.");
        }
    }

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameStarted || !canSpawn) return;

        float currentSpeed = GameManager.Instance.gameSpeed;
        float currentSpawnRate = distanceBetweenPillars / currentSpeed;
        currentSpawnRate = Mathf.Max(minSpawnRate, currentSpawnRate);

        if (timer < currentSpawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            SpawnPillar();
        }
    }

    void SpawnPillar()
    {
        if (pillarPrefab == null) return;

        GameObject newPillar = Instantiate(pillarPrefab, transform.position, Quaternion.identity);

        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= mutantChance)
        {
            MutantPillar mutantScript = newPillar.GetComponent<MutantPillar>();
            
            if (mutantScript != null)
            {
                mutantScript.enabled = true;
                Debug.Log("Mutant pillar spawned!");
            }
        }
    }
}
