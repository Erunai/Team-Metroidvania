using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;

    private FadeInOut fadeInOut;
    private void Start()
    {
        fadeInOut = FindAnyObjectByType<FadeInOut>().instance;
    }
    public void startGame(int sceneIndex)
    {
        // Fix: Get FadeInOut instance using FindObjectOfType
        float waitTime = fadeInOut.GetFadeDuration();
        StartCoroutine(Wait(waitTime));
        SceneManager.LoadScene(sceneIndex);
    }

    // Coroutine to wait for fade in/out
    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
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
