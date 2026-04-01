using System.Collections;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public GameObject normalWall; // Kéo NormalWall vào đây
    public GameObject ghostWall; // Kéo GhostWall vào đây

    public Transform WallSpawnPoint; // Điểm xuất hiện của tường

    public float spawnInterval = 3f;
    private bool isSpawning = false;

    void OnEnable()
    {
        isSpawning = true;
        StartCoroutine(SpawnWallRoutine());
    }

    void OnDisable()
    {
        StopSpawning();
    }

    IEnumerator SpawnWallRoutine()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnWall();
        }
    }

    void SpawnWall()
    {
        if (normalWall == null || ghostWall == null || WallSpawnPoint == null) return;

        // Tỉ lệ 50% spawn normalWall, 50% spawn ghostWall
        GameObject wallPrefab = Random.value > 0.5f ? normalWall : ghostWall;
        
        GameObject spawnedWall = Instantiate(wallPrefab, WallSpawnPoint.position, Quaternion.identity);

        // Đảm bảo tường có WallMovement để di chuyển
        if (spawnedWall.GetComponent<WallMovement>() == null)
        {
            spawnedWall.AddComponent<WallMovement>();
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
    }
}

public class WallMovement : MonoBehaviour
{
    public float speedMultiplier = 4f;
    public float deadZone = -90f;
    public bool canMove = true;

    void Update()
    {
        if (!canMove || GameManager.Instance == null || !GameManager.Instance.isGameStarted) return;

        float currentSpeed = GameManager.Instance.gameSpeed;
        
        transform.position += Vector3.left * currentSpeed * speedMultiplier * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
