using UnityEngine;
using UnityEngine.Tilemaps; // QUAN TRỌNG: Phải có dòng này để dùng Tilemap

public class TNTLogic : MonoBehaviour
{
    [Header("Cấu hình nổ")]
    public int mau = 2;
    public float lucNo = 25f;
    public float banKinhNo = 1.5f;
    public LayerMask layerAnhHuong; // Nhớ tích chọn Default, Player và Layer của Tilemap

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hammer"))
        {
            mau--;
            if (mau == 1)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
                Debug.Log("Chạm lần 1: Sắp nổ!");
            }
            else if (mau <= 0)
            {
                Explode();
            }
        }
    }

    void Explode()
    {
        // 1. Quét vật thể trong bán kính nổ
        Collider2D[] vatThe = Physics2D.OverlapCircleAll(transform.position, banKinhNo, layerAnhHuong);

        foreach (Collider2D obj in vatThe)
        {
            // A. PHÁ ĐÁ PREFAB (Nếu có tag Ground)
            if (obj.CompareTag("Ground") && obj.GetComponent<Tilemap>() == null)
            {
                obj.gameObject.SetActive(false);
                continue;
            }

            // B. PHÁ TILEMAP ĐƯỜNG ĐI
            Tilemap tilemap = obj.GetComponent<Tilemap>();
            if (tilemap != null)
            {
                // Tìm vị trí ô (Tile) dựa trên vị trí vụ nổ
                Vector3Int oTrungTam = tilemap.WorldToCell(transform.position);

                // Xóa ô ở giữa và các ô xung quanh (phạm vi 1 ô)
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        Vector3Int oCanXoa = new Vector3Int(oTrungTam.x + x, oTrungTam.y + y, 0);
                        tilemap.SetTile(oCanXoa, null); // Xóa Tile bằng cách đặt nó về null
                    }
                }
                continue;
            }

            // C. HẤT VĂNG NGƯỜI CHƠI
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null && obj.CompareTag("Player"))
            {
                Vector2 huongNo = (Vector2)obj.transform.position - (Vector2)transform.position;
                if (huongNo == Vector2.zero) huongNo = Vector2.up;
                rb.AddForce(huongNo.normalized * lucNo, ForceMode2D.Impulse);
            }
        }

        if (GameIntro.Instance != null)
        {
            GameIntro.Instance.OnTNTExploded();
        }

        Destroy(gameObject);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, banKinhNo);
    }
}