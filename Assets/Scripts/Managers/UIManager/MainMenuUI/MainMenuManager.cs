using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("StoryScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
