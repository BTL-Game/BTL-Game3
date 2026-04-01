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
    public float blizzardChance = 0.1f;
    public float blizzardCheckInterval = 5f;
    public float blizzardDuration = 10f;
    public float blizzardColdRate = 5f;
    public float blizzardWarningDuration = 5f;
    public GameObject blizzardEffect;
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
                isActive = false;
                FreezePlayer();
            }
        }
        UpdateWarningBlink();
    }
    private void UpdateWarningBlink()
    {
        if (warningText != null && warningText.gameObject.activeInHierarchy)
        {
            Color c = warningText.color;
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
    }
    public void IncreaseCold(float amount)
    {
        if (!isActive) return;
        currentCold += amount;
        currentCold = Mathf.Clamp(currentCold, 0, maxCold);
        UpdateUI();
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
    }
    private void CancelGracePeriod()
    {
        isGracePeriod = false;
        UpdateUI();
    }
    public void FreezePlayer()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameStarted)
        {
            if (warningText != null) warningText.gameObject.SetActive(false);
            BabyDragonMovement playerMovement = Object.FindFirstObjectByType<BabyDragonMovement>();
            if (playerMovement != null)
            {
                playerMovement.Die();
            }
            else
            {
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
            if (currentCold >= 80f)
            {
                warningText.gameObject.SetActive(true);
                warningText.text = "WARNINGS: TOO COLD!";
                warningText.color = new Color(1f, 0.5f, 0f);
            }
            else
            {
                warningText.gameObject.SetActive(false);
            }
        }
    }
}
