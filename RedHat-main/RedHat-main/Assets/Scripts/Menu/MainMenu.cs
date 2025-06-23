using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = FindAnyObjectByType<SoundManager>();
    }
    // ����� ��� ������ "������"
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // ��������� ����� � �������� 1 (GameScene)
        soundManager.PlayButtonClick();
    }

    // ����� ��� ������ "�����"
    public void QuitGame()
    {
        soundManager.PlayButtonClick();
        Application.Quit(); // ��������� ����������

        // � ��������� Unity ��� �� ���������, ������� ������� ���
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}