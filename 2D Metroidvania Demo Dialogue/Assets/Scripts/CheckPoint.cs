using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckPoint : MonoBehaviour
{
    Light2D light2D;
    bool activated;
    public void Start()
    {
        light2D = GetComponentInChildren<Light2D>();
        light2D.color = Color.green;
        deactivateCheckPoint();
    }
    /*
     * TODO: Give visual feedback when a player walks on a checkpoint -- e.g., change the sprite of a checkpoint to indicate that it has been activated.
     */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("New checkpoint activated");
            CheckPointController.instance.DeactivateCheckPoints();

            activateCheckPoint();

            CheckPointController.instance.SetSpawnPoint(transform.position);
        }
    }

    private void activateCheckPoint()
    {
        // Visual activation of checkpoint
        activated = true;
        light2D.enabled = true;
    }
    public void deactivateCheckPoint()
    {
        // Visual deactivation of checkpoint
        activated = false;
        light2D.enabled = false;
    }
}
