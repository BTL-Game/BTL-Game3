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

    // Hàm nhận và đổi Texture từ MapData
    public void ChangeTexture(Texture2D newTex)
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
            Debug.Log($"ParallaxBackground ({gameObject.name}): Đã tự động gọi lại GetComponent<MeshRenderer>() vì bị miss.");
        }
            
        if (meshRenderer != null)
        {
            if (newTex != null)
            {
                meshRenderer.enabled = true; // Bật lại hiển thị nếu layer này từng bị tắt trước đó
                meshRenderer.material.mainTexture = newTex;
                Debug.Log($"ParallaxBackground ({gameObject.name}): Đã cập nhật thành công texture mới -> {newTex.name}");
            }
            else
            {
                meshRenderer.enabled = false; // Ẩn luôn background này đi nếu Map mới không có ảnh
                Debug.Log($"ParallaxBackground ({gameObject.name}): Không có Texture, đã TẮT layer này.");
            }
        }
        else
        {
            Debug.LogError($"ParallaxBackground ({gameObject.name}): KHÔNG tìm thấy thành phần MeshRenderer nào trên GameObject này!");
        }
    }
}
