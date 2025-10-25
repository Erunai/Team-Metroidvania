using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    public void startGame(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void OpenSettings()
    {
        if (settingsMenu.activeInHierarchy.Equals(false))
        {
            settingsMenu.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    public void quitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
