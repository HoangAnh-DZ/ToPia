using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc có dòng này để load lại màn chơi

public class FallingLogic : MonoBehaviour
{
    [Header("Cấu hình vực thẳm")]
    public float yThreshold = -10f; // Độ cao mà dưới mức này được coi là rơi xuống vực
    public float timeToReset = 2f;  // Thời gian chờ trước khi reset (2 giây)

    private float timer = 0f;
    private bool isFalling = false;

    void Update()
    {
        // 1. Kiểm tra xem tọa độ Y của Tèo có thấp hơn ngưỡng cho phép không
        if (transform.position.y < yThreshold)
        {
            if (!isFalling)
            {
                isFalling = true;
                timer = 0f; // Bắt đầu đếm thời gian khi bắt đầu rơi
                Debug.Log("Tèo đang rơi vào vô tận... Đang đếm ngược 2s");
            }

            timer += Time.deltaTime; // Cộng dồn thời gian trôi qua

            // 2. Nếu thời gian rơi đã đủ 2 giây thì thực hiện Reset
            if (timer >= timeToReset)
            {
                ResetLevel();
            }
        }
        else
        {
            // Nếu Tèo vung cúp bám lại được và bay lên trên ngưỡng yThreshold
            isFalling = false;
            timer = 0f; // Reset bộ đếm
        }
    }

    void ResetLevel()
    {
        // Load lại cảnh (scene) hiện tại để bắt đầu lại từ đầu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}