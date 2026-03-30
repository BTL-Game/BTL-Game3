using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float scrollSpeed;
    public bool canRoll = true;
    private MeshRenderer meshRenderer;
    private float currentOffset = 0f;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (!canRoll || GameManager.Instance == null) return;

        float gameSpeed = GameManager.Instance.gameSpeed;

        currentOffset += scrollSpeed * (gameSpeed / 10f) * Time.deltaTime;

        meshRenderer.material.mainTextureOffset = new Vector2(currentOffset, 0);
    }

    public void ChangeTexture(Texture2D newTex)
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        if (meshRenderer != null)
        {
            if (newTex != null)
            {
                meshRenderer.enabled = true; 
                meshRenderer.material.mainTexture = newTex;
                Debug.Log($"[ParallaxBackground] Thay đổi texture thành: {newTex.name} trên {gameObject.name}");
            }
            else
            {
                meshRenderer.enabled = false; 
                Debug.Log($"[ParallaxBackground] Tắt render vì texture null trên {gameObject.name}");
            }
        }
    }
}
