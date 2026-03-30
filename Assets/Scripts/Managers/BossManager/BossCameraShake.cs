using UnityEngine;
using System.Collections;

public class BossCameraShake : MonoBehaviour
{
    public static BossCameraShake Instance { get; private set; }

    [Header("Shake Settings")]
    public float shakeDuration = 4f;
    public float shakeMagnitude = 2f;
    public float dampingSpeed = 2f;

    [Header("Audio Settings")]
    public AudioClip bossWarningClip;
    public AudioSource audioSource;

    private Vector3 initialPosition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    public void TriggerBossWarning()
    {
        if (bossWarningClip != null && audioSource != null)
        {
            audioSource.clip = bossWarningClip;
            audioSource.Play();
        }

        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float currentShakeDuration = shakeDuration;
        initialPosition = transform.localPosition;

        while (currentShakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime * dampingSpeed;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
