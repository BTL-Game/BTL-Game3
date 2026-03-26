using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool isGameStarted = false;
    [Header("Difficulty Settings")]
    public float gameSpeed = 5f;
    public float maxGameSpeed = 20f;
    public float speedMultiplier = 0.2f;

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
    }
}
