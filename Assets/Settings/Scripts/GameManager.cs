using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Playing, Respawning, Victory }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState currentState;

    void Awake()
    {
        if (Instance == null) Instance = this;
        currentState = GameState.Playing;
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        switch (currentState)
        {
            case GameState.Playing:
                Time.timeScale = 1;
                break;

            case GameState.Respawning:
                HandleRespawn();
                break;

            case GameState.Victory:
                Time.timeScale = 0;
                if (GoldCounter.Instance != null && GoldCounter.Instance.victoryPanel != null)
                {
                    GoldCounter.Instance.victoryPanel.SetActive(true);
                }
                break;
        }
    }

    private void HandleRespawn()
    {
        // Đồng bộ logic: Xóa vàng tạm thời chưa qua checkpoint
        GoldCounter.tempDestroyedPositions.Clear();

        // Trả số vàng về mốc đã lưu tại Checkpoint gần nhất
        GoldCounter.Instance.currentGold = GoldCounter.savedGold; // Logic static sẽ giữ giá trị này

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}