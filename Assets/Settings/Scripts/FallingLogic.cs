using UnityEngine;
using System.Collections;

public class FallingLogic : MonoBehaviour
{
    public float yThreshold = -10f;
    public float timeToReset = 2f;
    private float timer = 0f;
    private bool isFalling = false;
    public GameObject movementArrows;
    private bool isHiding = false;

    void Update()
    {
        // Chỉ cho phép ẩn mũi tên khi đã qua đoạn chào hỏi
        if (GameIntro.canMove && movementArrows != null && movementArrows.activeSelf && !isHiding)
        {
            if (Input.GetMouseButtonDown(0)) StartCoroutine(HideArrowsRoutine());
        }

        if (transform.position.y < yThreshold)
        {
            if (!isFalling) { isFalling = true; timer = 0f; }
            timer += Time.deltaTime;
            if (timer >= timeToReset)
            {
                GameManager.Instance.ChangeState(GameState.Respawning);
            }
        }
        else { isFalling = false; timer = 0f; }
    }

    IEnumerator HideArrowsRoutine()
    {
        isHiding = true;
        yield return new WaitForSeconds(0.5f);
        if (movementArrows != null)
        {
            movementArrows.SetActive(false);
            // Kích hoạt bước tiếp theo trong chuỗi: TNT Tut
            if (GameIntro.Instance != null) GameIntro.Instance.RequestShowTNTTutorial();
        }
        isHiding = false;
    }
}