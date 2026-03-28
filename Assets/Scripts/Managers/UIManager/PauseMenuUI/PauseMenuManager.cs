using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuMangaer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool isGamePaused = false;
    public GameObject PausedMenuUI;
    public AudioClip mainMenuMusic;

    void OnPause()
    {
        if (GameManager.Instance == null || !GameManager.Instance.isGameStarted) return;

        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        PausedMenuUI.SetActive(false);
    }

    void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
        // Show pause menu UI here
        PausedMenuUI.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadHome()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        if (MusicManager.instance != null && mainMenuMusic != null)
        {
            MusicManager.instance.ChangeMusicWithFade(mainMenuMusic);
        }
        SceneManager.LoadScene("MainMenuScene");
    }
}
