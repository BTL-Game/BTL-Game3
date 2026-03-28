using UnityEngine;
using UnityEngine.InputSystem;

public class BabyDragonMovement : MonoBehaviour
{
    [SerializeField] private float flappingStrength = 20f;
    private Rigidbody2D rigidBody2D;
    private bool isDead = false;
    private Animator animator;
    public GameObject ShieldObject;
    private SpriteRenderer spriteRenderer;

    // Collectible states
    private bool hasShield = false;
    private bool isInvincible = false;
    
    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead) return;

        bool falling = rigidBody2D.linearVelocityY < 0;
        animator.SetBool("isFalling", falling);
    }

    void OnJump(InputValue value)
    {
        if (isDead) return;
        
        // Allow jumping only after the intro sequence is finished.
        if (GameManager.Instance != null && !GameManager.Instance.isGameStarted) return;

        if (PauseMenuMangaer.isGamePaused) return;

        if (value.isPressed)
        {
            Flap();
        }
    }

    void Flap()
    {
        rigidBody2D.linearVelocity = new Vector2(rigidBody2D.linearVelocityX, flappingStrength);
        animator.SetTrigger("isFlapping");
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("Collided with: " + other.gameObject.name);
        if (other.CompareTag("Obstacle"))
        {
            if (isInvincible)
            {
                // Go through everything, destroy the obstacle perhaps, or just ignore it
                return;
            }

            if (hasShield)
            {
                // Endure one collision
                ShieldObject.SetActive(false); // Hide the shield visual
                hasShield = false;
                Destroy(other.gameObject); // Destroy the obstacle we hit
                Debug.Log("Shield destroyed!");
                return;
            }

            Debug.Log("GAME OVER!");
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        Debug.Log("Baby Dragon has died.");
        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.simulated = false;

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

        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameStarted = false;
            GameManager.Instance.CompleteBossPhase();
        }

        BossManager boss = Object.FindFirstObjectByType<BossManager>();
        if (boss != null)
        {
            boss.FreezeBossForGameOver();
        }
    }

    public void ActivateShield()
    {
        hasShield = true;
        ShieldObject.SetActive(true);
        Debug.Log("Shield activated!");
    }

    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityRoutine(duration));
        }
    }

    private System.Collections.IEnumerator InvincibilityRoutine(float duration)
    {
        isInvincible = true;
        Debug.Log("Invincibility activated!");

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.5f;
            spriteRenderer.color = c;
        }

        yield return new WaitForSeconds(duration);
        
        isInvincible = false;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }

        Debug.Log("Invincibility ended!");
    }
}
