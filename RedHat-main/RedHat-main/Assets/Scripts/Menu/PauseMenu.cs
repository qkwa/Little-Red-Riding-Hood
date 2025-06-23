using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private string mainMenuScene = "MainMenu";

    private bool isPaused = false;
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ����������� ���� ��� �������� �����
            if (soundManager != null)
                soundManager.PlayButtonClick();
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void TogglePause()
    {
        // ����������� ���� ��� �������� �����
        if (soundManager != null)
            soundManager.PlayButtonClick();
        if (isPaused)
            Resume();

        else
            Pause();
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;

        // ����������� ���� ��� �������� �����
        if (soundManager != null)
            soundManager.PlayButtonClick();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;

        // ����������� ���� ��� ����������� ����
        if (soundManager != null)
            soundManager.PlayButtonClick();
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        // ����������� ���� ����� ��������� ����
        if (soundManager != null)
            soundManager.PlayButtonClick();

        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        // ����������� ���� ����� �������
        if (soundManager != null)
            soundManager.PlayButtonClick();

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}