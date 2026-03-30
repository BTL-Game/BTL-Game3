using UnityEngine;

public class PlayerIntro : MonoBehaviour
{
    [Header("Movement Settings")]
    public Vector3 targetPosition = new Vector3(-51.3f, 0, 0); 
    public float introSpeed = 20f; 
    public float startOffset = 50f; 

    [Header("Animation")]
    public string animationToPlay = "intro_animation"; 

    private Animator anim;
    private bool isIntroFinished = false;

    public void StartIntro()
    {
        isIntroFinished = false;
        anim = GetComponent<Animator>();

        transform.position = targetPosition - new Vector3(startOffset, 0, 0);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) 
        {
            rb.simulated = false; // Tắt mô phỏng vật lý lúc đang Intro
            rb.linearVelocity = Vector2.zero; // Xóa gia tốc tụt dư thừa nếu có
        }

        BabyDragonMovement moveScript = GetComponent<BabyDragonMovement>(); 
        if (moveScript != null) 
        {
            moveScript.enabled = false;
            moveScript.RemoveAllEffects(); // Xóa sạch mọi hiệu ứng cũ (khiên, tàng hình, đảo trọng lực)
        }

        if (anim != null)
        {
            anim.Play(animationToPlay);
        }
    }

    void Start()
    {
        StartIntro();
    }

    void Update()
    {
        if (isIntroFinished) return;

        Vector3 basePosition = Vector3.MoveTowards(transform.position, targetPosition, introSpeed * Time.deltaTime);
        float hover = Mathf.Sin(Time.time * 3.0f) * 0.02f; 
        transform.position = new Vector2(basePosition.x, basePosition.y + hover);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            FinishIntro();
        }
    }

    void FinishIntro()
    {
        isIntroFinished = true;
        transform.position = targetPosition; 

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) 
        {
            rb.simulated = true;
            rb.gravityScale = 4f;
        }

        MonoBehaviour moveScript = GetComponent<BabyDragonMovement>();
        if (moveScript != null) moveScript.enabled = true;

        if (anim != null)
        {

            anim.Play("falling_animation"); 
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameStarted = true;
        }

        this.enabled = false; // Thay vì Destroy, ta chỉ vô hiệu hóa để có thể dùng lại khi chuyển map
    }
}