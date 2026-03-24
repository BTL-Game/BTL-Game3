using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject glowObject; // Kéo PlayButton_Glow vào đây

    void Start() => glowObject.SetActive(false); // Lúc đầu ẩn đi

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (glowObject != null) glowObject.SetActive(true);
        transform.localScale = Vector3.one * 1.05f; // Nút chính to nhẹ lên
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (glowObject != null) glowObject.SetActive(false);
        transform.localScale = Vector3.one; // Trả về cỡ cũ
    }
}