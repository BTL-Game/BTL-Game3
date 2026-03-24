using UnityEngine;
using UnityEngine.InputSystem;

public class BabyDragonMovement : MonoBehaviour
{
    [SerializeField] private float flappingStrength = 20f;
    private Rigidbody2D rb;
    private bool isDead = false;
    private Animator animator;
    
    void Awake() // Đổi Start thành Awake
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (isDead) return;

        bool falling = rb.linearVelocityY < 0;
        animator.SetBool("isFalling", falling);
    }
    void OnJump(InputValue value)
    {
        if (isDead) return;
        
        // THÊM DÒNG NÀY: Chỉ cho phép nhảy khi Intro đã xong
        if (GameManager.Instance != null && !GameManager.Instance.isGameStarted) return;

        if (value.isPressed)
        {
            Flap();
        }
    }

    void Flap()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocityX, flappingStrength);
        animator.SetTrigger("isFlapping");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("Collided with: " + other.gameObject.name);
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("GAME OVER!");
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        Debug.Log("Baby Dragon is Dead :((");
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
        ParallaxBackground[] parallax = Object.FindObjectsByType<ParallaxBackground>(FindObjectsSortMode.None);
        foreach (ParallaxBackground bg in parallax)
        {
            bg.canRoll = false;
        }
        PillarMovement[] pillars = Object.FindObjectsByType<PillarMovement>(FindObjectsSortMode.None);
        foreach (PillarMovement pillar in pillars)
        {
            pillar.canMove = false;
        }

        PillarSpawner spawner = Object.FindFirstObjectByType<PillarSpawner>();
        if (spawner != null)
        {
            spawner.canSpawn = false;
        }
    }
}
