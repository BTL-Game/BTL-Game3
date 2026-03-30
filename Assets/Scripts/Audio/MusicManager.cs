using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
public class MusicManager : MonoBehaviour
{

    public static MusicManager instance;
    public AudioSource musicSource;

    [Header("Music Settings")]
    public float fadeDuration = 0.5f;
    public float originalVolume;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            originalVolume = musicSource.volume;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeMusicWithFade(AudioClip newMusic)
    {
        if (musicSource.clip == newMusic) return;

        StartCoroutine(FadeTrack(newMusic));
    }
    private IEnumerator FadeTrack(AudioClip newMusic)
    {
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(originalVolume, 0, timer / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newMusic;
        musicSource.Play();

        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0, originalVolume, timer / fadeDuration);
            yield return null;
        }

        musicSource.volume = originalVolume;
    }
}
