using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Bắt buộc để điều khiển ánh sáng
using UnityEngine.SceneManagement;
public class GoldCounter : MonoBehaviour
{
    public static GoldCounter Instance;

    [Header("Cấu hình UI")]
    public GameObject victoryPanel;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI notifyText;

    [Header("Cấu hình Game")]
    public int targetGold = 5;
    public Light2D globalLight; // Kéo Global Light 2D vào đây

    public static int savedGold = 0;
    public static Vector3? checkpointPos = null;
    public static List<Vector3> destroyedPositions = new List<Vector3>();
    public static List<Vector3> tempDestroyedPositions = new List<Vector3>();

    public int currentGold;
    private bool hasTriggeredDarkness = false; // Tránh chạy hiệu ứng tối nhiều lần

    void Awake()
    {
        Instance = this;
        currentGold = savedGold;
        UpdateGoldUI();

        if (victoryPanel != null) victoryPanel.SetActive(false);
        if (notifyText != null) notifyText.gameObject.SetActive(false);

        if (checkpointPos != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) player.transform.position = checkpointPos.Value;
        }
    }

    public void AddGold(Vector3 pos)
    {
        currentGold++;
        if (!tempDestroyedPositions.Contains(pos))
        {
            tempDestroyedPositions.Add(pos);
        }
        UpdateGoldUI();

        // HIỆU ỨNG KHI NHẶT VIÊN VÀNG ĐẦU TIÊN
        if (currentGold == 1 && !hasTriggeredDarkness)
        {
            StartCoroutine(FirstGoldMissionEffect());
            hasTriggeredDarkness = true;
        }
    }

    IEnumerator FirstGoldMissionEffect()
    {
        // 1. Làm map tối đi 1 tí
        if (globalLight != null)
        {
            float elapsed = 0;
            float duration = 1.5f;
            float startIntensity = globalLight.intensity;
            float targetIntensity = 0.4f; // Chỉnh độ tối tùy ý

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                yield return null;
            }
        }

        // 2. Hiện thông báo nhiệm vụ
        if (notifyText != null)
        {
            notifyText.gameObject.SetActive(true);
            notifyText.text = "Thu thập đủ vàng và tới đích!";
            yield return new WaitForSeconds(3f);
            notifyText.gameObject.SetActive(false);
        }
    }

    public void UpdateGoldUI()
    {
        if (goldText != null)
        {
            // Nó sẽ tự hiện: x0/5 hoặc x0/10 tùy vào số m gõ ở Inspector
            goldText.text = "x" + currentGold ;
            
        }
    }

    public void SaveCheckpoint(Vector3 pos)
    {
        checkpointPos = new Vector3(pos.x, pos.y + 1.5f, pos.z);
        savedGold = currentGold;
        foreach (Vector3 tempPos in tempDestroyedPositions)
        {
            if (!destroyedPositions.Contains(tempPos)) destroyedPositions.Add(tempPos);
        }
        tempDestroyedPositions.Clear();
    }

    public bool CheckGold() => currentGold >= targetGold;

    public void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0;
            checkpointPos = null;
            savedGold = 0;
            destroyedPositions.Clear();
            tempDestroyedPositions.Clear();
        }
    }

    public void ShowNotEnoughGoldMessage()
    {
        if (notifyText != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashMessage());
        }
    }

    IEnumerator FlashMessage()
    {
        notifyText.gameObject.SetActive(true);
        notifyText.text = "Chưa đủ sính lễ Tèo ơi!";
        yield return new WaitForSeconds(2f);
        notifyText.gameObject.SetActive(false);
    }
    public void RestartGame()
    {
        // 1. Reset danh sách vàng tạm thời (những viên nhặt SAU checkpoint)
        // Vì m chưa tới Checkpoint mới, nên những viên này phải được hiện ra lại
        tempDestroyedPositions.Clear();

        // 2. Trả số vàng về giá trị đã lưu tại Checkpoint gần nhất
        currentGold = savedGold;

        // 3. Load lại Scene để Unity khôi phục lại đất cát, vật thể
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}