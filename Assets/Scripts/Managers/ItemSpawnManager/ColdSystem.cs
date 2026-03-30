using UnityEngine;
using UnityEngine.UI;

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

        IncreaseCold(coldRate * Time.deltaTime);
    }

    public void SetActive(bool active, float rate = 2f)
    {
        isActive = active;
        coldRate = rate;

        if (coldSliderObject != null)
        {
            coldSliderObject.SetActive(isActive);
        }

        if (!isActive)
        {
            ResetCold();
        }
    }

    public void IncreaseCold(float amount)
    {
        if (!isActive) return;

        currentCold += amount;
        currentCold = Mathf.Clamp(currentCold, 0, maxCold);
        UpdateUI();

        if (currentCold >= maxCold)
        {
            FreezePlayer();
        }
    }

    public void DecreaseCold(float amount)
    {
        if (!isActive) return;

        currentCold -= amount;
        currentCold = Mathf.Clamp(currentCold, 0, maxCold);
        UpdateUI();
    }

    public void FreezePlayer()
    {

        if (GameManager.Instance != null && GameManager.Instance.isGameStarted)
        {

            GameManager.Instance.TriggerGameOver(); 
        }
    }

    public void ResetCold()
    {
        currentCold = 0f;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coldSlider != null)
        {

            coldSlider.value = currentCold / maxCold; 

        }
    }
}
