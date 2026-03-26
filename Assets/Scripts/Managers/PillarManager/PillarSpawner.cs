using UnityEngine;

public class PillarSpawner : MonoBehaviour
{
    public GameObject pillarPrefab;
    public float minFireLength = 5f;
    public float maxFireLength = 15f;
    private float timer = 0f;
    public bool canSpawn = true;
    public float distanceBetweenPillars = 15f;
    public float minSpawnRate = 0.3f;

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
        GameObject newPillar = Instantiate(pillarPrefab, new Vector3(93, 0, 0), Quaternion.identity);
        PillarSizeController sizeController = newPillar.GetComponent<PillarSizeController>();

        if (sizeController != null)
        {
            float randomTop = Random.Range(minFireLength, maxFireLength);
            float randomBottom = Random.Range(minFireLength, maxFireLength);
            sizeController.SetupPillar(randomTop, randomBottom);
        }
    }
}
