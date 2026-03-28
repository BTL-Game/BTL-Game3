using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [Header("Cấu hình Sprite")]
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
        
        // Lưu trạng thái vào máy
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        
        UpdateUI();
        ApplyAudioSettings();
    }

    void UpdateUI()
    {
        // Đổi hình ảnh cái loa
        buttonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
    }

    void ApplyAudioSettings()
    {
        // Tắt/Mở toàn bộ âm thanh trong game
        AudioListener.pause = isMuted;
    }
}