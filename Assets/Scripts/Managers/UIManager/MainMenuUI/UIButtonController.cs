using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject glowObject; // Assign PlayButton_Glow here.

    void Start() => glowObject.SetActive(false); // Hidden by default.

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (glowObject != null) glowObject.SetActive(true);
        transform.localScale = Vector3.one * 1.05f; // Slightly enlarge main button.
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (glowObject != null) glowObject.SetActive(false);
        transform.localScale = Vector3.one; // Restore original scale.
    }
}