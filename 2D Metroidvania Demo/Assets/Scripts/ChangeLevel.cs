using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private bool left;
    [SerializeField] private bool instant;
    [SerializeField] private float walkTimer;
    [SerializeField] private int sceneIndex;
    private int moveMultiplier;
    private float walkCounter;

    private void Start()
    {
        //playerController = FindAnyObjectByType<PlayerController>();
        moveMultiplier = left ? -1 : 1;
        walkCounter = walkTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (instant)
            {
                SceneManager.LoadScene(sceneIndex);
            }
            StartCoroutine(ChangeLevels());
        }
    }

    private IEnumerator ChangeLevels()
    {
        //playerController.transform.parent = null;
        PlayerController.instance.Walk(moveMultiplier);
        yield return new WaitForSeconds(walkTimer);
        SceneManager.LoadScene(sceneIndex);
    }
}
