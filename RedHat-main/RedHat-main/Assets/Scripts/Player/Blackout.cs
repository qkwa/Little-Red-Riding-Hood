using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Blackout : MonoBehaviour
{
    public Image darkImage;
    public float fadeDuration = 1f;
    private bool isFading = false;

    public void StartBlackout(bool isTeleporting, Transition transition = null)
    {
        StartCoroutine(FadeDarkScreen(isTeleporting, transition));
    }

    IEnumerator FadeDarkScreen(bool isTeleporting, Transition targetTransition)
    {
        isFading = true;

        // Затемнение
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            darkImage.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, timer / fadeDuration));
            yield return null;
        }

        // Телепортация
        if (isTeleporting && targetTransition != null)
        {
            targetTransition.Transfer();
            // Осветление
            timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                darkImage.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, timer / fadeDuration));
                yield return null;
            }

            isFading = false;
        }

        
    }
}