using UnityEngine;

public class MutantPillar : MonoBehaviour
{
    [Header("Mutant Settings")]
    public float speed = 2f;
    public float height = 2f;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * speed) * height;

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}