using UnityEngine;

public class PlayerIntro : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 targetPosition = new Vector3(-51.3f, 0, 0); // Final position before gameplay starts
    public float introSpeed = 20f; // Flight speed during intro
    public float startOffset = 50f; // Spawn distance off-screen

    [Header("Animation")]
    public string animationToPlay = "intro_animation"; // Exact Animator state name

    private Animator anim;
    private bool isIntroFinished = false;

    void Start()
    {
        // 1. Cache Animator.
        anim = GetComponent<Animator>();

        // 2. Place dragon off-screen to the left.
        transform.position = targetPosition - new Vector3(startOffset, 0, 0);

        // 3. Disable physics and controls so intro plays safely.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        MonoBehaviour moveScript = GetComponent<BabyDragonMovement>(); 
        if (moveScript != null) moveScript.enabled = false;

        // Force intro animation state.
        if (anim != null)
        {
            anim.Play(animationToPlay);
        }
    }

    void Update()
    {
        if (isIntroFinished) return;

        // Move to target position with a subtle hover effect.
        Vector3 basePosition = Vector3.MoveTowards(transform.position, targetPosition, introSpeed * Time.deltaTime);
        float hover = Mathf.Sin(Time.time * 3.0f) * 0.02f; 
        transform.position = new Vector2(basePosition.x, basePosition.y + hover);

        // Finish intro when close enough to target.
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            FinishIntro();
        }
    }

    void FinishIntro()
    {
        isIntroFinished = true;
        transform.position = targetPosition; // Snap to exact target.

        // Re-enable physics and controls for gameplay.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        MonoBehaviour moveScript = GetComponent<BabyDragonMovement>();
        if (moveScript != null) moveScript.enabled = true;
        
        if (anim != null)
        {
            // Return to a gameplay-ready animation state.
            anim.Play("falling_animation"); 
        }

        // Notify GameManager to start gameplay spawning.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameStarted = true;
        }

        // Disable this intro behavior after completion.
        Destroy(this);
    }
}