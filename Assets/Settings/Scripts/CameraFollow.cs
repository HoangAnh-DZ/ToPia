using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Kéo Player vào ô này
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 2, -10); // Để camera cao hơn Player một chút

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            // Lerp giúp camera di chuyển mượt, không bị giật theo vật lý
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        }
    }
}