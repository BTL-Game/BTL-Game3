using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SanitySystem : MonoBehaviour
{
    public static SanitySystem Instance { get; private set; }
    [Header("Sanity Status")]
    public float currentSanity = 100f;
    public float maxSanity = 100f;
    public float sanityDecreaseRate = 2f; 
    [Header("UI Reference")]
    public GameObject sanitySliderObject;
    public Slider sanitySlider;
    public TMP_Text warningText;
    public float warningBlinkSpeed = 3f;
    [Header("Grace Period Settings")]
    public float graceDuration = 5f;
    private bool isGracePeriod = false;
    private float graceTimer = 0f;
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
        if (!isGracePeriod)
        {
            DecreaseSanity(sanityDecreaseRate * Time.deltaTime);
        }
        else
        {
            graceTimer -= Time.deltaTime;
            if (warningText != null)
            {
                warningText.text = $"SANITY DEPLETED!\nTIME REMAINING: {Mathf.Ceil(graceTimer)}s";
                warningText.color = Color.red;
            }
            if (graceTimer <= 0)
            {
                isActive = false;
                LoseSanity();
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
        sanityDecreaseRate = rate;
        if (sanitySliderObject != null)
        {
            sanitySliderObject.SetActive(isActive);
        }
        if (active)
        {
            ResetSanity();
        }
        else
        {
            if (warningText != null) warningText.gameObject.SetActive(false);
        }
    }
    public void IncreaseSanity(float amount)
    {
        if (!isActive) return;
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        UpdateUI();
        if (currentSanity > 0 && isGracePeriod)
        {
            CancelGracePeriod();
        }
    }
    public void DecreaseSanity(float amount)
    {
        if (!isActive) return;
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        UpdateUI();
        if (currentSanity <= 0 && !isGracePeriod)
        {
            StartGracePeriod();
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
    public void LoseSanity()
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
    public void ResetSanity()
    {
        currentSanity = maxSanity;
        isGracePeriod = false;
        if (warningText != null) warningText.gameObject.SetActive(false);
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (sanitySlider != null)
        {
            sanitySlider.value = currentSanity / maxSanity; 
        }
        if (warningText != null && !isGracePeriod)
        {
            if (currentSanity <= 30f)
            {
                warningText.gameObject.SetActive(true);
                warningText.text = "WARNING: LOW SANITY!";
                warningText.color = new Color(0.8f, 0f, 1f);
            }
            else
            {
                warningText.gameObject.SetActive(false);
            }
        }
    }
}
