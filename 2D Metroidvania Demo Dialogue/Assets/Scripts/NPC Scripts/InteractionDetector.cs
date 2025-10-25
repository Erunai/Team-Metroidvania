using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    public GameObject interactionIcon;

    private IInteractable inRange;

    void Start()
    {
        if (interactionIcon != null) interactionIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            inRange = interactable;
            if (interactionIcon) interactionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out IInteractable interactable) && interactable == inRange)
        {
            inRange = null;
            if (interactionIcon) interactionIcon.SetActive(false);
        }
    }

    void Update()
    {
        // Keep icon in sync if target becomes invalid while still in trigger
        if (inRange != null && !inRange.CanInteract())
        {
            inRange = null;
            if (interactionIcon) interactionIcon.SetActive(false);
        }

        if (inRange != null && Input.GetKeyDown(KeyCode.E))
        {
            if (interactionIcon) interactionIcon.SetActive(false); // hide as soon as we start
            inRange.Interact();
        }
    }
}