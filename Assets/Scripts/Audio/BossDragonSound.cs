using UnityEngine;

public class BossDragonSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip dragonFireSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void PlayDragonFire()
    {
        if (dragonFireSound != null)
        {
            audioSource.PlayOneShot(dragonFireSound);
        }
    }
}