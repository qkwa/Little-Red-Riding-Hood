using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour
{
    [SerializeField] Text textField;
    private PlayerAnimationController playerAnimationController;

    private void Start()
    {
        playerAnimationController = FindAnyObjectByType<PlayerAnimationController>();
    }

    public void ShowWinMessage()
    {
        if (textField != null)
        {
            textField.text = "Победа";
            // Устанавливаем непрозрачный белый цвет (альфа = 1.0)
            textField.color = new Color(textField.color.r, textField.color.g, textField.color.b, 1f);
            StartCoroutine(RestartAfterDelay());
        }
    }

    public void ShowLoseMessage()
    {
        if (textField != null)
        {
            textField.text = "Поражение";
            // Устанавливаем непрозрачный белый цвет (альфа = 1.0)
            textField.color = new Color(textField.color.r, textField.color.g, textField.color.b, 1f);
        }
    }

    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        playerAnimationController.RestartGame();
    }
}