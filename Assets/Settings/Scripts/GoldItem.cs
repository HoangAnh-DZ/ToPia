using UnityEngine;

public class GoldItem : MonoBehaviour
{
    private bool isCollected = false;

    void Start()
    {
        // Nếu vị trí này nằm trong danh sách Vĩnh viễn hoặc Tạm thời (khi mới load scene lại)
        if (GoldCounter.destroyedPositions.Contains(transform.position) ||
            GoldCounter.tempDestroyedPositions.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected && (collision.CompareTag("Player") || collision.CompareTag("Hammer")))
        {
            isCollected = true;
            if (GoldCounter.Instance != null)
            {
                GoldCounter.Instance.AddGold(transform.position);
            }
            Destroy(gameObject);
        }
    }
}