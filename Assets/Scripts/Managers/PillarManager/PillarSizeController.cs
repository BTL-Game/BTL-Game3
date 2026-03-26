using UnityEngine;

public class PillarSizeController : MonoBehaviour
{
    public SpriteRenderer topFire;
    public SpriteRenderer bottomFire;
    public BoxCollider2D topCollider;
    public BoxCollider2D bottomCollider;

    public void SetupPillar(float topLength, float bottomLength)
    {
        ApplySegmentSize(topFire, topCollider, topLength);
        ApplySegmentSize(bottomFire, bottomCollider, bottomLength);
    }

    private static void ApplySegmentSize(SpriteRenderer fire, BoxCollider2D collider, float length)
    {
        fire.size = new Vector2(fire.size.x, length);
        collider.size = new Vector2(collider.size.x, length);
        collider.offset = new Vector2(0f, length / 2f);
    }
}
