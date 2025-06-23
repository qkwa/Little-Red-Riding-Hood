using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowLevelName : MonoBehaviour
{
    [SerializeField] private string levelName;
    [SerializeField] private Text title;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float displayDuration = 3f;

    private bool isAnimating = false;
    private Coroutine currentAnimation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAnimating)
        {
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(ShowAndHideText());
        }
    }

    private IEnumerator ShowAndHideText()
    {
        isAnimating = true;

        // Установка текста
        title.text = levelName;

        // Плавное появление
        yield return FadeText(0f, 1f);

        // Ждем пока текст виден
        yield return new WaitForSeconds(displayDuration);

        // Плавное исчезание
        yield return FadeText(1f, 0f);

        isAnimating = false;
        Destroy(gameObject);
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        Color textColor = title.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            title.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }
    }
}