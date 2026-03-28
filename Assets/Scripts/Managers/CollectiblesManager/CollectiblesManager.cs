using UnityEngine;

[System.Serializable]
public class CollectibleData
{
    public string name;
    public GameObject prefab;
    [Range(0, 100)]
    public int spawnWeight;
}
public class CollectiblesManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum ItemType { Coin, Shield, Invincible, Speed }
    public ItemType type;
    public float value = 10f; // Điểm hoặc thời gian tác dụng

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra Tag ở đối tượng va chạm HOẶC đối tượng cha của nó
        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            Debug.Log("==> Đã xác nhận va chạm với PLAYER: " + other.name);
            
            // Gọi hiệu ứng
            ApplyEffect(other.gameObject);
            
            // Xóa vật phẩm: Xóa chính đối tượng chứa script này
            // Đảm bảo script này nằm ở root của Prefab vật phẩm (Coin/Shield...)
            Destroy(gameObject);
        }
        else 
        {
            // Debug này giúp bạn biết vật phẩm đang va chạm với cái gì (ví dụ: Obstacle)
            Debug.Log("Va chạm với đối tượng không phải Player: " + other.name + " Tag: " + other.tag);
        }
    }

    void ApplyEffect(GameObject player)
    {
        // Tìm script ở đối tượng va chạm, nếu không thấy thì tìm ở cha/con
        BabyDragonMovement dragon = player.GetComponentInParent<BabyDragonMovement>();
        if (dragon == null) dragon = player.GetComponentInChildren<BabyDragonMovement>();

        if (dragon == null)
        {
            Debug.LogError("!!! KHÔNG tìm thấy script BabyDragonMovement trên Player!");
            return;
        }

        Debug.Log("--- Đang áp dụng hiệu ứng: " + type.ToString() + " ---");

        switch (type)
        {
            case ItemType.Coin:
                if (GameManager.Instance != null) {
                    GameManager.Instance.AddScore(10);
                }
                break;
            case ItemType.Shield:
                dragon.ActivateShield();
                break;
            case ItemType.Invincible:
                // Đảm bảo biến 'value' trong Inspector khác 0
                dragon.ActivateInvincibility(value); 
                break;
            case ItemType.Speed:
                if (GameManager.Instance != null) {
                    GameManager.Instance.SpeedUp(5f, value);
                }
                break;
        }
    }
}
