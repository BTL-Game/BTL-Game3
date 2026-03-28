using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject glowObject;
    public AudioClip hoverSound;
    private AudioSource audioSource;

    void Start()
    {
        if (glowObject != null) glowObject.SetActive(false);
                audioSource = GetComponent<AudioSource>();
        if (audioSource == null) 
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
                audioSource.playOnAwake = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (glowObject != null) glowObject.SetActive(true);
        transform.localScale = Vector3.one * 1.05f;

        if (hoverSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (glowObject != null) glowObject.SetActive(false);
        transform.localScale = Vector3.one;
    }
}