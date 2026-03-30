using UnityEngine;

public class IceItem : MonoBehaviour
{
    public enum Type { Flame, Snowflake, MutantSnowflake }
    public Type itemType;

    [Header("Effect Value")]
    public float effectAmount = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            if (ColdSystem.Instance == null) return;

            switch (itemType)
            {
                case Type.Flame:
                    ColdSystem.Instance.DecreaseCold(effectAmount);

                    break;
                case Type.Snowflake:
                    ColdSystem.Instance.IncreaseCold(effectAmount);

                    break;
                case Type.MutantSnowflake:
                    ColdSystem.Instance.FreezePlayer();

                    break;
            }

            Destroy(gameObject);
        }
    }
}