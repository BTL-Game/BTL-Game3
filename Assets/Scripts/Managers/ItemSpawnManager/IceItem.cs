using UnityEngine;

public class IceItem : MonoBehaviour
{
    public enum Type { Flame, Snowflake, MutantSnowflake }
    public Type itemType;

    [Header("Effect Value")]
    public float effectAmount = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có va chạm trúng Player không
        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            if (ColdSystem.Instance == null) return;

            switch (itemType)
            {
                case Type.Flame:
                    ColdSystem.Instance.DecreaseCold(effectAmount);
                    Debug.Log($"Đã nhặt Lửa! Giảm {effectAmount} độ lạnh.");
                    break;
                case Type.Snowflake:
                    ColdSystem.Instance.IncreaseCold(effectAmount);
                    Debug.Log($"Đã nhặt Tuyết! Tăng {effectAmount} độ lạnh.");
                    break;
                case Type.MutantSnowflake:
                    ColdSystem.Instance.FreezePlayer();
                    Debug.Log("Đã nhặt phải Tuyết Đột Biến! Đóng băng ngay lập tức.");
                    break;
            }

            Destroy(gameObject);
        }
    }
}