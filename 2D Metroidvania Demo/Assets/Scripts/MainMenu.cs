using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void startGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
