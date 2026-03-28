using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Settings")]
    public int scoreValue = 10;
    public AudioClip collectSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        
        Debug.Log("Collected flame! Added " + scoreValue + " points.");

        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        Destroy(gameObject);
    }
}