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
    public enum ItemType { Coin, Shield, Invincible, Speed, GravityShift, Flame, Snowflake, MutantSnowflake}
    public ItemType type;
    public float value = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {

            ApplyEffect(other.gameObject);

            Destroy(gameObject);
        }
        else 
        {

        }
    }

    void ApplyEffect(GameObject player)
    {
        BabyDragonMovement dragon = player.GetComponentInParent<BabyDragonMovement>();
        if (dragon == null) dragon = player.GetComponentInChildren<BabyDragonMovement>();

        if (dragon == null)
        {

            return;
        }

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
                dragon.ActivateInvincibility(value); 
                break;
            case ItemType.Speed:
                if (GameManager.Instance != null) {
                    GameManager.Instance.SpeedUp(5f, value);
                }
                break;
            case ItemType.GravityShift:
                dragon.ActivateGravityShift(value);
                break;
            case ItemType.Flame:
                if (ColdSystem.Instance != null) ColdSystem.Instance.DecreaseCold(30f);
                break;
            case ItemType.Snowflake:
                if (ColdSystem.Instance != null) ColdSystem.Instance.IncreaseCold(20f);
                break;
            case ItemType.MutantSnowflake:
                if (ColdSystem.Instance != null) ColdSystem.Instance.IncreaseCold(20f);
                dragon.FreezeDragon(5f);
                break;
        }
    }
}
