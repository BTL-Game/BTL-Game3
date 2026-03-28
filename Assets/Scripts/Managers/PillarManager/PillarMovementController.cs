using UnityEngine;

public class PillarMovementController : MonoBehaviour
{
    public void SetupPillar(float yPosition)
    {
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
        
    }
}