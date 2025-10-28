using UnityEngine;

public class Mushroom_Wake_Up : MonoBehaviour
{
    private bool _isActive;
    private Transform _player;

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered wake-up zone.");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            // Trigger the wake-up animation
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("WakeUp");
                _isActive = true;
            }
        }
    }

    private void Update()
    {
        if (_isActive)
        {
            facePlayer();
        }
    }

    void facePlayer()
    {
        // If the player is left, set x scale to -1, else set to 1
        float xScale = _player.position.x < transform.position.x ? -1 : 1;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
    }
}