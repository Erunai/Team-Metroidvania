using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    // Should maybe make this a state machine change instead of a coroutine -- but this is simpler for now


    [SerializeField] private bool left; // Walk left or right when changing scenes
    [SerializeField] private bool instant; // Change scenes instantly on trigger enter -- should maybe be replaced by walkTimer = 0
    [SerializeField] private float walkTimer; // How long to walk before changing scenes
    [SerializeField] private int sceneIndex; // What scene index to change too
    private int _moveMultiplier;
    private float _walkCounter;

    private void Start()
    {
        //playerController = FindAnyObjectByType<PlayerController>();
        _moveMultiplier = left ? -1 : 1;
        _walkCounter = walkTimer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (instant) // could replace with walkTimer = 0
            {
                SceneManager.LoadScene(sceneIndex);
            }
            StartCoroutine(ChangeLevels());
        }
    }

    private IEnumerator ChangeLevels()
    {
        //playerController.transform.parent = null;
        // PlayerController.instance.Walk(moveMultiplier);
        yield return new WaitForSeconds(walkTimer);
        SceneManager.LoadScene(sceneIndex);
    }
}
