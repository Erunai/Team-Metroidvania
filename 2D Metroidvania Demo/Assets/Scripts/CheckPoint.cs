using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckPoint : MonoBehaviour
{
    Light2D light2D;
    bool notActive;
    public void Start()
    {
        light2D = GetComponentInChildren<Light2D>();
        light2D.color = Color.green;
        deactivateCheckPoint(); // Start checkpoint as deactivated
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("New checkpoint activated");
            // Disable other checkpoints
            CheckPointController.instance.DeactivateCheckPoints();

            // Enable Checkpoint
            if (notActive) activateCheckPoint(); // Only activate if not already activated - Maybe this should be for the entire code block?
            CheckPointController.instance.SetSpawnPoint(transform.position);
        }
    }

    private void activateCheckPoint()
    {
        // Visual activation of checkpoint
        notActive = false;
        light2D.color = Color.green; // Set light color to Green
    }
    public void deactivateCheckPoint()
    {
        // Visual deactivation of checkpoint
        notActive = true;
        light2D.color = Color.red; // Set light color to Red
    }
}
