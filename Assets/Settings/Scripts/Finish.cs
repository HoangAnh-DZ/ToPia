using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GoldCounter.Instance.CheckGold())
            {
                // Yêu cầu GameManager chuyển sang trạng thái thắng
                GameManager.Instance.ChangeState(GameState.Victory);
            }
            else
            {
                GoldCounter.Instance.ShowNotEnoughGoldMessage();
            }
        }
    }
}