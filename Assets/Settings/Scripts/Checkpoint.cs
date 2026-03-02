using UnityEngine;
public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GoldCounter.Instance.SaveCheckpoint(transform.position);
            Debug.Log("Checkpoint Activated!"); // Kiểm tra Console để biết đã lưu chưa
        }
    }
}