using UnityEngine;

public class BabyDragonSound : MonoBehaviour
{
    private AudioSource audioSource;

    // Khai báo riêng từng loại âm thanh
    [Header("Audio Clips")]
    public AudioClip flappingSound;
    public AudioClip deathSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Gọi hàm này tại frame vỗ cánh
    public void PlayFlappingSound()
    {
        if (flappingSound != null)
        {
            audioSource.PlayOneShot(flappingSound);
        }
    }

    // Gọi hàm này khi rồng chết
    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}