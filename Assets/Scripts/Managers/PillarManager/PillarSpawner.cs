using UnityEngine;
public class PillarSpawner : MonoBehaviour
{
    public GameObject pillarPrefab;
    [Header("Spawn Chance (%)")]
    [Range(0f, 100f)] 
    public float mutantChance = 5f;
    public float minLength = -10f;
    public float maxLength = 10f;
    private float timer = 0f;
    public bool canSpawn = true;
    public float distanceBetweenPillars = 15f;
    public float minSpawnRate = 0.3f;
    public void SetPillarPrefab(GameObject prefab)
    {
        pillarPrefab = prefab;
        if (prefab != null)
        {
        }
        else
        {
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
        float randomYOffset = Random.Range(minLength, maxLength);
        Transform topPillar = newPillar.transform.Find("PillarTop");
        Transform bottomPillar = newPillar.transform.Find("PillarBottom");
        Transform itemSpawnLocation = newPillar.transform.Find("ItemPoint");
        if (topPillar != null)
        {
            topPillar.localPosition += new Vector3(0, randomYOffset, 0);
        }
        if (bottomPillar != null)
        {
            bottomPillar.localPosition += new Vector3(0, randomYOffset, 0);
        }
        if (itemSpawnLocation != null)
        {
            itemSpawnLocation.localPosition += new Vector3(0, randomYOffset, 0);
        }
        if (topPillar == null && bottomPillar == null)
        {
            newPillar.transform.position += new Vector3(0, randomYOffset, 0);
        }
        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= mutantChance)
        {
            MutantPillar mutantScript = newPillar.GetComponent<MutantPillar>();
            if (mutantScript != null)
            {
                mutantScript.enabled = true;
            }
        }
    }
}
