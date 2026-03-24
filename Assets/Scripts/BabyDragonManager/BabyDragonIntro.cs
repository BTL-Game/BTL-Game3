using UnityEngine;

public class PlayerIntro : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    public Vector3 targetPosition = new Vector3(-51.3f, 0, 0); // Vị trí đứng để bắt đầu chơi
    public float introSpeed = 5f;                         // Tốc độ bay vào màn hình
    public float startOffset = 50f;                        // Khoảng cách xuất hiện ngoài màn hình

    [Header("Hoạt ảnh")]
    public string animationToPlay = "intro_animation"; // TÊN CHÍNH XÁC của State đập cánh trong Animator

    private Animator anim;
    private bool isIntroFinished = false;

    void Start()
    {
        // 1. Lấy thành phần Animator
        anim = GetComponent<Animator>();

        // 2. Đặt rồng ra ngoài màn hình bên trái
        transform.position = targetPosition - new Vector3(startOffset, 0, 0);

        // 3. VÔ HIỆU HÓA Vật lý và Điều khiển (Để rồng không bị rơi khi đang Intro)
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Thay 'BabyDragonMovement' bằng tên chính xác Script di chuyển của bạn
        MonoBehaviour moveScript = GetComponent<BabyDragonMovement>(); 
        if (moveScript != null) moveScript.enabled = false;

        // --- ĐÂY LÀ CÁCH 2: ÉP CHƠI HOẠT ẢNH ---
        if (anim != null)
        {
            anim.Play(animationToPlay);
        }
    }

    void Update()
    {
        if (isIntroFinished) return;

        // Di chuyển rồng tiến về vị trí mục tiêu (Chỉ giữ lại đoạn tính toán có hover)
        Vector3 basePosition = Vector3.MoveTowards(transform.position, targetPosition, introSpeed * Time.deltaTime);
        float hover = Mathf.Sin(Time.time * 3.0f) * 0.02f; 
        transform.position = new Vector2(basePosition.x, basePosition.y + hover);

        // Kiểm tra nếu đã đến nơi
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            FinishIntro();
        }
    }

    void FinishIntro()
    {
        isIntroFinished = true;
        transform.position = targetPosition; // Khớp vị trí chuẩn

        // BẬT LẠI Vật lý và Điều khiển để bắt đầu chơi thực sự
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        MonoBehaviour moveScript = GetComponent<BabyDragonMovement>();
        if (moveScript != null) moveScript.enabled = true;
        
        if (anim != null)
            {
                // Ép Animator quay về trạng thái mặc định hoặc trạng thái Falling
                // để các mũi tên (Transitions) bắt đầu có tác dụng trở lại.
                anim.Play("falling_animation"); 
            }

        // Thông báo cho GameManager bắt đầu đẻ cột lửa
        if (GameManager.Instance != null)
        {
            GameManager.Instance.isGameStarted = true;
        }

        // Tự hủy script này sau khi hoàn thành nhiệm vụ
        Destroy(this);
    }
}