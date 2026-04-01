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
    [Header("Frozen State")]
    public Sprite frozenSprite;
    private bool hasShield = false;
    private bool isInvincible = false;
    private bool isGravityReversed = false;
    private bool isFrozen = false;
    public bool isCursed = false;
    [Header("Ghost Form")]
    public bool isGhostForm = false;
    void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (isDead || isFrozen) return;
        bool falling = isGravityReversed ? rigidBody2D.linearVelocityY > 0 : rigidBody2D.linearVelocityY < 0;
        animator.SetBool("isFalling", falling);
    }
    void OnJump(InputValue value)
    {
        if (isDead || isFrozen) return;
        if (GameManager.Instance != null && !GameManager.Instance.isGameStarted) return;
        if (PauseMenuManager.isGamePaused) return;
        if (value.isPressed)
        {
            Flap();
        }
    }
    void OnChange(InputValue value)
    {
        if (isDead || isFrozen) return;
        if (GameManager.Instance != null && !GameManager.Instance.isGameStarted) return;
        if (PauseMenuManager.isGamePaused) return;
        if (MapManager.Instance != null && !(MapManager.Instance.currentMap is GhostMapData)) return;
        if (value.isPressed)
        {
            isGhostForm = !isGhostForm;
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = isGhostForm ? 0.4f : 1f;
                spriteRenderer.color = c;
            }
        }
    }
    void Flap()
    {
        rigidBody2D.linearVelocity = new Vector2(rigidBody2D.linearVelocityX, flappingStrength);
        animator.SetTrigger("isFlapping");
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Obstacle"))
        {
            if (isGhostForm) return;
            if (isInvincible)
            {
                return;
            }
            if (hasShield)
            {
                ShieldObject.SetActive(false); 
                hasShield = false;
                Destroy(other.gameObject); 
                return;
            }
            Die();
        }
        else if (other.CompareTag("Spectral"))
        {
            if (!isGhostForm) return;
            if (isInvincible)
            {
                return;
            }
            if (hasShield)
            {
                ShieldObject.SetActive(false); 
                hasShield = false;
                Destroy(other.gameObject); 
                return;
            }
            Die();
        }
        else if (other.CompareTag("Border")){
            Die();
        }
    }
    public void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetBool("isDead", true);
        rigidBody2D.linearVelocity = Vector2.zero;
        rigidBody2D.simulated = false;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
    public void ActivateShield()
    {
        hasShield = true;
        ShieldObject.SetActive(true);
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
    }
    private Coroutine gravityRoutine;
    public void ActivateGravityShift(float duration)
    {
        if (gravityRoutine != null)
        {
            StopCoroutine(gravityRoutine);
        }
        else
        {
            isGravityReversed = true;
            rigidBody2D.gravityScale *= -1;
            flappingStrength *= -1;
            if (spriteRenderer != null) spriteRenderer.flipY = true;
        }
        gravityRoutine = StartCoroutine(GravityShiftRoutine(duration));
    }
    private System.Collections.IEnumerator GravityShiftRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (isGravityReversed) 
        {
            isGravityReversed = false;
            rigidBody2D.gravityScale = Mathf.Abs(rigidBody2D.gravityScale);
            flappingStrength = Mathf.Abs(flappingStrength);
            if (spriteRenderer != null) spriteRenderer.flipY = false;
        }
        gravityRoutine = null;
    }
    private Coroutine freezeRoutine;
    private Coroutine curseRoutine;
    private float preFreezeGravity = 3f; 
    public void FreezeDragon(float duration)
    {
        if (freezeRoutine != null)
        {
            StopCoroutine(freezeRoutine);
        }
        else
        {
            preFreezeGravity = rigidBody2D.gravityScale;
        }
        freezeRoutine = StartCoroutine(FreezeRoutine(duration));
    }
    private System.Collections.IEnumerator FreezeRoutine(float duration)
    {
        isFrozen = true;
        rigidBody2D.linearVelocity = Vector2.zero;
        if (rigidBody2D.gravityScale != 0f) {
            preFreezeGravity = rigidBody2D.gravityScale;
        }
        rigidBody2D.gravityScale = 0f;
        animator.enabled = false; 
        if (spriteRenderer != null && frozenSprite != null)
        {
            spriteRenderer.sprite = frozenSprite;
        }
        yield return new WaitForSeconds(duration);
        isFrozen = false;
        rigidBody2D.gravityScale = preFreezeGravity;
        animator.enabled = true; 
        animator.speed = 1f;
        freezeRoutine = null;
    }
    public void ActivateCurse(float duration)
    {
        if (curseRoutine != null)
        {
            StopCoroutine(curseRoutine);
        }
        else
        {
            rigidBody2D.gravityScale *= 2f; 
        }
        curseRoutine = StartCoroutine(CurseRoutine(duration));
    }
    private System.Collections.IEnumerator CurseRoutine(float duration)
    {
        isCursed = true;
        float originalSanityRate = 2f;
        if (SanitySystem.Instance != null)
        {
            originalSanityRate = SanitySystem.Instance.sanityDecreaseRate;
            SanitySystem.Instance.sanityDecreaseRate = 5f;
        }
        
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.r = 0.5f; c.g = 0.5f; c.b = 0.5f;
            spriteRenderer.color = c;
        }
        yield return new WaitForSeconds(duration);
        isCursed = false;
        rigidBody2D.gravityScale /= 2f;
        
        if (SanitySystem.Instance != null && SanitySystem.Instance.sanityDecreaseRate == 5f)
        {
            SanitySystem.Instance.sanityDecreaseRate = originalSanityRate;
        }
        
        curseRoutine = null;
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.r = 1f; c.g = 1f; c.b = 1f;
            spriteRenderer.color = c;
        }
    }
    public void RemoveAllEffects()
    {
        StopAllCoroutines();
        gravityRoutine = null;
        freezeRoutine = null;
        curseRoutine = null;
        hasShield = false;
        if (ShieldObject != null)
        {
            ShieldObject.SetActive(false);
        }
        isInvincible = false;
        if (isCursed)
        {
            if (SanitySystem.Instance != null && SanitySystem.Instance.sanityDecreaseRate == 5f)
            {
                SanitySystem.Instance.sanityDecreaseRate = 2f;
            }
        }
        isCursed = false;
        isGhostForm = false;
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            c.r = 1f; c.g = 1f; c.b = 1f;
            spriteRenderer.color = c;
        }
        if (isGravityReversed)
        {
            isGravityReversed = false;
            rigidBody2D.gravityScale = Mathf.Abs(rigidBody2D.gravityScale);
            flappingStrength = Mathf.Abs(flappingStrength);
            if (spriteRenderer != null) spriteRenderer.flipY = false;
        }
        if (isFrozen)
        {
            isFrozen = false;
            rigidBody2D.gravityScale = preFreezeGravity != 0 ? preFreezeGravity : 3f;
            animator.enabled = true;
            animator.speed = 1f;
        }
    }
}
