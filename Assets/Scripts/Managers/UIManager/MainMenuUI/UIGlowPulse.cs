using UnityEngine;
using UnityEngine.UI;

public class UIGlowPulse : MonoBehaviour
{
    public float pulseSpeed = 4f;
    public float minScale = 1.05f;
    public float maxScale = 1.2f;
    private Image img;

    void Awake() => img = GetComponent<Image>();

    void Update()
    {
        float wave = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
        transform.localScale = Vector3.one * Mathf.Lerp(minScale, maxScale, wave);
        
        if (img != null) {
            Color c = img.color;
            c.a = Mathf.Lerp(0.2f, 0.6f, wave);
            img.color = c;
        }
    }
}