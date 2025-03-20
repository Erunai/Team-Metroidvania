using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    /*
     * TODO: Give visual feedback when a player walks on a checkpoint -- e.g., change the sprite of a checkpoint to indicate that it has been activated.
     */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckPointController.instance.DeactivateCheckPoints();

            // Activate checkpoint -- Once we get visual feedback, we can implement this method

            CheckPointController.instance.SetSpawnPoint(transform.position);
        }
    }

    public void resetCheckPoint()
    {
        // Reset older checkpoint to its original state -- Once we get visual feedback, we can implement this method
    }
}
