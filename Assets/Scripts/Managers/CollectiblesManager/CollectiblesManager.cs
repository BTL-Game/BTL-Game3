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
    public enum ItemType { Coin, Shield, Invincible, Speed }
    public ItemType type;
    public float value = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || (other.transform.parent != null && other.transform.parent.CompareTag("Player")))
        {
            Debug.Log("==> Confirmed collision with PLAYER: " + other.name);
            
            ApplyEffect(other.gameObject);
            
            Destroy(gameObject);
        }
        else 
        {
            Debug.Log("Collided with non-Player object: " + other.name + " Tag: " + other.tag);
        }
    }

    void ApplyEffect(GameObject player)
    {
        BabyDragonMovement dragon = player.GetComponentInParent<BabyDragonMovement>();
        if (dragon == null) dragon = player.GetComponentInChildren<BabyDragonMovement>();

        if (dragon == null)
        {
            Debug.LogError("!!! Could not find BabyDragonMovement script on Player!");
            return;
        }

        Debug.Log("--- Applying effect: " + type.ToString() + " ---");

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
        }
    }
}
