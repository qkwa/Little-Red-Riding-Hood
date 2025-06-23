using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
    }
    // Метод для кнопки "Играть"
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // Загружаем сцену с индексом 1 (GameScene)
        soundManager.PlayButtonClick();
    }

    // Метод для кнопки "Выход"
    public void QuitGame()
    {
        soundManager.PlayButtonClick();
        Application.Quit(); // Закрываем приложение

        // В редакторе Unity это не сработает, поэтому выводим лог
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}