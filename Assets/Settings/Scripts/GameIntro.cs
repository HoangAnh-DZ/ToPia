using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameIntro : MonoBehaviour
{
    public Image fadeImage;
    public GameObject movementArrows; // Kéo một Object cha chứa cả 2 mũi tên vào đây
    public float fadeDuration = 2f;

    void Start()
    {
        if (movementArrows != null) movementArrows.SetActive(true); // Lúc đầu ẩn đi

        if (fadeImage != null)
        {
            StartCoroutine(FadeFromBlack());
        }
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

        
    }
}