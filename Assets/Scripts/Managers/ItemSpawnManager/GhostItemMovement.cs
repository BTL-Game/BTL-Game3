using UnityEngine;
public class GhostItemMovement : MonoBehaviour
{
    public float baseMoveSpeed = 4f;
    public float amplitude = 2f;
    public float frequency = 2f;
    public float deadZone = -80f;
    private float startY;
    private float offsetX;
    public bool canMove = true;
    void Start()
    {
        startY = transform.position.y;
        offsetX = Random.Range(0f, 100f);
        amplitude = Random.Range(4f, 8f);
        frequency = Random.Range(1.5f, 3.5f);
    }
    void Update()
    {
        if (!canMove || GameManager.Instance == null || !GameManager.Instance.isGameStarted) return;
        float currentSpeed = GameManager.Instance.gameSpeed;
        float newY = startY + Mathf.Sin((Time.time + offsetX) * frequency) * amplitude;
        transform.position += Vector3.left * currentSpeed * baseMoveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
