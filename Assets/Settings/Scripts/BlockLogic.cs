using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    public enum BlockType { Soi, Da, DaCung }
    public BlockType loaiDa;

    public int mau = 3; // Số lần chạm/đập để vỡ
    public float lucDapToiThieu = 5f; // Chỉ tính là 1 lần đập nếu lực vung đủ mạnh

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Chỉ nhận va chạm từ Cúp (Tag Hammer)
        if (collision.gameObject.CompareTag("Hammer"))
        {
            // 2. Tính toán lực va chạm thực tế
            float lucVaCham = collision.relativeVelocity.magnitude;

            // 3. Nếu lực vung búa lớn hơn ngưỡng cho phép mới trừ máu
            if (lucVaCham >= lucDapToiThieu)
            {
                TakeDamage();
                Debug.Log("Đã đập 1 lần! Máu còn: " + mau + " | Lực đập: " + lucVaCham);
            }
        }
    }

    void TakeDamage()
    {
        if (loaiDa == BlockType.DaCung) return; // Đá cứng không vỡ

        mau--;

        // Hiệu ứng đổi màu để người chơi biết đá sắp vỡ
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (mau == 2) renderer.color = Color.gray;
        if (mau == 1) renderer.color = Color.red;

        if (mau <= 0)
        {
            // Tạo hiệu ứng vỡ hoặc âm thanh ở đây nếu muốn
            Destroy(gameObject);
        }
    }
}