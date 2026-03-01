using UnityEngine;
using TMPro;
using System.Collections; // Bắt buộc có cái này để dùng Coroutine

public class GoldCounter : MonoBehaviour
{
    public static GoldCounter Instance;

    [Header("Cấu hình UI")]
    public GameObject victoryPanel;   // Bảng thắng cuộc (Panel)
    public TextMeshProUGUI goldText;  // Chữ hiển thị số vàng (x0, x1...)
    public TextMeshProUGUI notifyText; // Chữ thông báo "Chưa đủ vàng"

    [Header("Cấu hình Game")]
    public int currentGold = 0;
    public int targetGold = 5;

    void Awake()
    {
        Instance = this;
        // Lúc đầu game ẩn bảng thắng và chữ thông báo đi
        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (notifyText != null) notifyText.gameObject.SetActive(false);
    }

    public void AddGold()
    {
        currentGold++;
        UpdateGoldUI();
    }

    void UpdateGoldUI()
    {
        if (goldText != null) goldText.text = "x" + currentGold;
    }

    public bool CheckGold()
    {
        return currentGold >= targetGold;
    }

    public void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0; // Dừng game
        }
    }

    // Hàm hiện thông báo khi chưa đủ vàng
    public void ShowNotEnoughGoldMessage()
    {
        if (notifyText != null)
        {
            StopAllCoroutines(); // Dừng các lệnh ẩn cũ nếu m chạm liên tục
            StartCoroutine(FlashMessage());
        }
    }

    IEnumerator FlashMessage()
    {
        notifyText.gameObject.SetActive(true);
        
        yield return new WaitForSeconds(2f); // Hiện trong 2 giây

        notifyText.gameObject.SetActive(false);
    }
}