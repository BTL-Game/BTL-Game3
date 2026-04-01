using UnityEngine;
using UnityEngine.UI;
public class SoundToggle : MonoBehaviour
{
    [Header("Sprite Settings")]
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    public Image buttonImage;
    private bool isMuted = false;
    void Start()
    {
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        UpdateUI();
        ApplyAudioSettings();
    }
    public void ToggleMute()
    {
        isMuted = !isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        UpdateUI();
        ApplyAudioSettings();
    }
    void UpdateUI()
    {
        buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }
    void ApplyAudioSettings()
    {
        AudioListener.pause = isMuted;
    }
}
