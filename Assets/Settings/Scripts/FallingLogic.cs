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
        if (movementArrows != null && movementArrows.activeSelf && !isHiding)
        {
            if (Input.GetMouseButtonDown(0)) StartCoroutine(HideArrowsRoutine());
        }

        if (transform.position.y < yThreshold)
        {
            if (!isFalling) { isFalling = true; timer = 0f; }
            timer += Time.deltaTime;

            if (timer >= timeToReset)
            {
                // GỌI STATE MACHINE THAY VÌ TỰ LOAD
                GameManager.Instance.ChangeState(GameState.Respawning);
            }
        }
        else { isFalling = false; timer = 0f; }
    }

    IEnumerator HideArrowsRoutine()
    {
        isHiding = true;
        yield return new WaitForSeconds(0.5f);
        if (movementArrows != null) movementArrows.SetActive(false);
        isHiding = false;
    }
}