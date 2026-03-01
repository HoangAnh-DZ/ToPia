using UnityEngine;

public class GoldItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu chạm vào Player HOẶC Búa (Hammer)
        if (collision.CompareTag("Player") || collision.CompareTag("Hammer"))
        {
            if (GoldCounter.Instance != null)
            {
                GoldCounter.Instance.AddGold(); // Cộng vàng
                Destroy(gameObject); // Biến mất đồng vàng
            }
        }
    }
}