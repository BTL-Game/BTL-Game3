using UnityEngine;

public class PillarMovement : MonoBehaviour
{
    public float deadZone = -80f;
    public bool canMove = true;

    void Update()
    {
        if (!canMove || GameManager.Instance == null) return;

        float currentSpeed = GameManager.Instance.gameSpeed;
        transform.position += Vector3.left * currentSpeed * 4f * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
