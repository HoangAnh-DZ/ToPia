using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameIntro : MonoBehaviour
{
    public static GameIntro Instance;
    public static bool canMove = false;

    private static bool hasPlayedIntro = false;
    private static bool hasShownTNTTut = false;
    private static bool hasShownRockTut = false;

    // Thêm biến để biết TNT đã thực sự nổ chưa
    private static bool isTNTDone = false;
    private static bool isRockDone = false;

    [Header("UI References")]
    public Image fadeImage;
    public GameObject darkOverlay;
    public GameObject movementArrows;
    public GameObject tntTut;
    public GameObject rockTut;

    [Header("Settings")]
    public float fadeDuration = 1f; // Giảm xuống cho hồi sinh nhanh
    public float welcomeDuration = 3f;

    void Awake() { if (Instance == null) Instance = this; }

    void Start()
    {
        HideAllInitialUI();

        if (hasPlayedIntro)
        {
            canMove = true;
            if (fadeImage) fadeImage.gameObject.SetActive(false);

            // LOGIC FIX: Nếu hồi sinh mà TNT chưa nổ thì hiện lại Tut TNT
            if (!isTNTDone)
            {
                if (tntTut) tntTut.SetActive(true);
                hasShownTNTTut = true;
            }
            // Nếu TNT nổ rồi nhưng đá chưa vỡ thì hiện lại Tut Đá
            else if (!isRockDone)
            {
                if (rockTut) rockTut.SetActive(true);
                hasShownRockTut = true;
            }
            return;
        }

        canMove = false;
        if (fadeImage) StartCoroutine(FadeFromBlack());
    }

    void HideAllInitialUI()
    {
        if (movementArrows) movementArrows.SetActive(false);
        if (tntTut) tntTut.SetActive(false);
        if (rockTut) rockTut.SetActive(false);
        if (darkOverlay) darkOverlay.SetActive(false);
    }

    IEnumerator FadeFromBlack()
    {
        float timer = 0;
        Color tempColor = fadeImage.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            tempColor.a = 1 - (timer / fadeDuration);
            fadeImage.color = tempColor;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
        StartCoroutine(ShowWelcomeRoutine());
    }

    IEnumerator ShowWelcomeRoutine()
    {
        if (darkOverlay) darkOverlay.SetActive(true);
        yield return new WaitForSeconds(welcomeDuration);
        if (darkOverlay) darkOverlay.SetActive(false);
        if (movementArrows) movementArrows.SetActive(true);
        canMove = true;
        hasPlayedIntro = true;
    }

    public void RequestShowTNTTutorial()
    {
        if (!isTNTDone && tntTut)
        {
            tntTut.SetActive(true);
            hasShownTNTTut = true;
        }
    }

    public void OnTNTExploded()
    {
        isTNTDone = true; // Đánh dấu TNT đã nổ vĩnh viễn
        if (tntTut) tntTut.SetActive(false);
        if (!isRockDone && rockTut)
        {
            rockTut.SetActive(true);
            hasShownRockTut = true;
        }
    }

    public void HideRockTutorial()
    {
        isRockDone = true; // Đánh dấu đá đã vỡ vĩnh viễn
        if (rockTut) rockTut.SetActive(false);
    }
}