using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // QUAN TRỌNG: Thêm dòng này để dùng TextMeshPro

public class GameOverManager : MonoBehaviour
{
    public static bool isGameOver = false;
    public GameObject GameOverUI;
    public TextMeshProUGUI scoreText; // Kéo đối tượng chữ "Score:" vào đây
    public AudioClip mainMenuMusic;

    // Hàm này sẽ được gọi khi rồng chết
    public void Setup(int score)
    {
        if (GameOverUI != null) 
        {
            GameOverUI.SetActive(true); 
        }
        isGameOver = true;
        scoreText.text = "Score: " + score.ToString(); 
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadHome()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        if (MusicManager.instance != null)
        {
            MusicManager.instance.ChangeMusicWithFade(mainMenuMusic);
        }
        SceneManager.LoadScene("MainMenuScene");
    }
}