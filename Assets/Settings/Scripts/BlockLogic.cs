using UnityEngine;

public class BlockLogic : MonoBehaviour
{
    public enum BlockType { Soi, Da, DaCung }
    public BlockType loaiDa;

    public int mau = 3;
    public float lucDapToiThieu = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            float lucVaCham = collision.relativeVelocity.magnitude;

            if (lucVaCham >= lucDapToiThieu)
            {
                TakeDamage();
                Debug.Log("Đã đập! Máu còn: " + mau);
            }
        }
    }

    void TakeDamage()
    {
        if (loaiDa == BlockType.DaCung) return;

        mau--;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (mau == 2) renderer.color = Color.gray;
        if (mau == 1) renderer.color = Color.red;

        if (mau <= 0)
        {
            // Khi đá vỡ, ra lệnh cho GameIntro ẩn hướng dẫn đi
            if (GameIntro.Instance != null)
            {
                GameIntro.Instance.HideRockTutorial();
            }
            Destroy(gameObject);
        }
    }
}