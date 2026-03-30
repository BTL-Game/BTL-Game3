using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColdSystem : MonoBehaviour
{
    public static ColdSystem Instance { get; private set; }

    [Header("Cold Status")]
    public float currentCold = 0f;
    public float maxCold = 100f;
    public float coldRate = 2f; 

    [Header("UI Reference")]
    public GameObject coldSliderObject;
    public Slider coldSlider;
    public TMP_Text warningText;
    public float warningBlinkSpeed = 3f;

    [Header("Grace Period Settings")]
    public float graceDuration = 5f;
    private bool isGracePeriod = false;
    private float graceTimer = 0f;

    [Header("Blizzard Settings")]
    public float blizzardChance = 0.1f; // 10% chance
    public float blizzardCheckInterval = 5f; // Check every 5 seconds
    public float blizzardDuration = 10f; // Blizzard lasts for 10 seconds
    public float blizzardColdRate = 5f;
    public float blizzardWarningDuration = 5f;
    public GameObject blizzardEffect; // The visual effect Object
    
    private bool isBlizzardActive = false;
    private bool isBlizzardWarning = false;
    private float normalColdRate;
    private float blizzardCheckTimer = 0f;
    private float activeBlizzardTimer = 0f;
    private float warningBlizzardTimer = 0f;

    private bool isActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (!isActive || GameManager.Instance == null || !GameManager.Instance.isGameStarted) return;

        HandleBlizzard();

        if (!isGracePeriod)
        {
            IncreaseCold(coldRate * Time.deltaTime);
        }
        else
        {
            graceTimer -= Time.deltaTime;
            if (warningText != null)
            {
                warningText.text = $"NEED FLAMES ASAP!\n TIME REMAININGS: {Mathf.Ceil(graceTimer)}s";
                warningText.color = Color.red;
            }

            if (graceTimer <= 0)
            {
                isActive = false; // Ngừng tính toán
                FreezePlayer();   // Hết 5 giây không nhặt được lửa thì chết
            }
        }

        UpdateWarningBlink();
    }

    private void UpdateWarningBlink()
    {
        if (warningText != null && warningText.gameObject.activeInHierarchy)
        {
            Color c = warningText.color;
            // Nhấp nháy alpha từ 0.2 đến 1.0
            c.a = 0.2f + Mathf.PingPong(Time.time * warningBlinkSpeed, 0.8f);
            warningText.color = c;
        }
    }

    public void SetActive(bool active, float rate = 2f)
    {
        isActive = active;
        coldRate = rate;
        normalColdRate = rate;

        if (coldSliderObject != null)
        {
            coldSliderObject.SetActive(isActive);
        }

        if (!isActive)
        {
            ResetCold();
            StopBlizzard();
        }
    }

    private void HandleBlizzard()
    {
        if (isBlizzardWarning)
        {
            warningBlizzardTimer -= Time.deltaTime;
            
            if (warningText != null && !isGracePeriod)
            {
                warningText.gameObject.SetActive(true);
                warningText.text = $"BLIZZARD INCOMING IN {Mathf.Ceil(warningBlizzardTimer)}s!";
                warningText.color = new Color(1f, 1f, 0f); // Màu vàng (Ice)
            }

            if (warningBlizzardTimer <= 0)
            {
                isBlizzardWarning = false;
                StartBlizzard();
            }
        }
        else if (isBlizzardActive)
        {
            activeBlizzardTimer -= Time.deltaTime;
            if (activeBlizzardTimer <= 0)
            {
                StopBlizzard();
            }
        }
        else
        {
            blizzardCheckTimer -= Time.deltaTime;
            if (blizzardCheckTimer <= 0)
            {
                blizzardCheckTimer = blizzardCheckInterval;
                if (Random.value <= blizzardChance)
                {
                    StartBlizzardWarning();
                }
            }
        }
    }

    private void StartBlizzardWarning()
    {
        isBlizzardWarning = true;
        warningBlizzardTimer = blizzardWarningDuration;
        Debug.Log("[ColdSystem] Sắp có bão tuyết! Cảnh báo trong 5 giây.");
    }

    private void StartBlizzard()
    {
        isBlizzardActive = true;
        activeBlizzardTimer = blizzardDuration;
        coldRate = blizzardColdRate;

        if (blizzardEffect != null)
        {
            blizzardEffect.SetActive(true);
        }
        
        Debug.Log("[ColdSystem] Bão tuyết bắt đầu! Rate lạnh tăng lên " + coldRate + "f/s");
    }

    private void StopBlizzard()
    {
        isBlizzardActive = false;
        coldRate = normalColdRate;

        if (blizzardEffect != null)
        {
            blizzardEffect.SetActive(false);
        }

        if (warningText != null && !isGracePeriod && currentCold < 80f)
        {
            warningText.gameObject.SetActive(false);
        }
        
        Debug.Log("[ColdSystem] Bão tuyết kết thúc! Rate lạnh trở về " + coldRate + "f/s");
    }

    public void IncreaseCold(float amount)
    {
        if (!isActive) return;

        currentCold += amount;
        currentCold = Mathf.Clamp(currentCold, 0, maxCold);
        UpdateUI();

        // Kích hoạt 5 giây sinh tử nếu đạt 100 độ lạnh
        if (currentCold >= maxCold && !isGracePeriod)
        {
            StartGracePeriod();
        }
    }

    public void DecreaseCold(float amount)
    {
        if (!isActive) return;

        currentCold -= amount;
        currentCold = Mathf.Clamp(currentCold, 0, maxCold);
        UpdateUI();

        // Thoát khỏi đếm ngược tử thần nếu người chơi nhặt được ngọn lửa giảm độ lạnh
        if (currentCold < maxCold && isGracePeriod)
        {
            CancelGracePeriod();
        }
    }

    private void StartGracePeriod()
    {
        isGracePeriod = true;
        graceTimer = graceDuration;
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
        }
        Debug.Log("[ColdSystem] Đạt 100 độ lạnh. Bắt đầu bộ đếm 5 giây ân hạn!");
    }

    private void CancelGracePeriod()
    {
        isGracePeriod = false;
        UpdateUI();
        Debug.Log("[ColdSystem] Đã thoát khỏi trạng thái ân hạn do độ lạnh được giảm xuống!");
    }

    public void FreezePlayer()
    {

        if (GameManager.Instance != null && GameManager.Instance.isGameStarted)
        {
            // Tắt UI đếm ngược
            if (warningText != null) warningText.gameObject.SetActive(false);

            // Tìm BabyDragonMovement và gọi hàm Die() trực tiếp để kích hoạt đầy đủ hiệu ứng + animation
            BabyDragonMovement playerMovement = Object.FindFirstObjectByType<BabyDragonMovement>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
            else
            {
                // Fallback nếu không tìm thấy script điều khiển rồng
                GameManager.Instance.TriggerGameOver(); 
            }
        }
    }

    public void ResetCold()
    {
        currentCold = 0f;
        isGracePeriod = false;
        if (warningText != null) warningText.gameObject.SetActive(false);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coldSlider != null)
        {

            coldSlider.value = currentCold / maxCold; 

        }

        if (warningText != null && !isGracePeriod && !isBlizzardWarning)
        {
            // Hiển thị cảnh báo nhỏ nếu độ lạnh từ 80 trở lên
            if (currentCold >= 80f)
            {
                warningText.gameObject.SetActive(true);
                warningText.text = "WARNINGS: TOO COLD!";
                warningText.color = new Color(1f, 0.5f, 0f); // Màu cam
            }
            else
            {
                warningText.gameObject.SetActive(false);
            }
        }
    }
}
