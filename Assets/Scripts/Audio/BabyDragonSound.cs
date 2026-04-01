using UnityEngine;
public class BabyDragonSound : MonoBehaviour
{
    private AudioSource audioSource;
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
    public void PlayFlappingSound()
    {
        if (flappingSound != null)
        {
            audioSource.PlayOneShot(flappingSound);
        }
    }
    public void PlayDeathSound()
    {
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
    }
}
