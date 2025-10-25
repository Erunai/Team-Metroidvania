using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPC3 : MonoBehaviour, IInteractable
{
    [Header("Data")]
    public NPCDialogue dialogueData;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image portraitImage;

    [Header("References")]
    [Tooltip("Drag the player object that has PlayerMovement here.")]
    public PlayerController playerMovement;

    private int index;
    private bool dialogueActive;
    private bool isTyping;
    private Coroutine typingRoutine;

    void Reset()
    {
        // If scene has a player with PlayerMovement, try to auto-find
        if (playerMovement == null)
            playerMovement = FindObjectOfType<PlayerController>();
    }

    // ---- IInteractable ----
    public bool CanInteract()
    {
        return dialogueData != null
               && dialogueData.dialogueLines != null
               && dialogueData.dialogueLines.Length > 0
               && !dialogueActive; // don’t re-trigger while already in dialogue
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        StartDialogue();
    }

    // ---- Flow ----
    private void StartDialogue()
    {
        dialogueActive = true;
        index = 0;

        if (playerMovement != null) playerMovement.SetFrozen(true);

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (nameText != null) nameText.SetText(dialogueData.npcName);
        if (portraitImage != null) portraitImage.sprite = dialogueData.npcPortrait;

        StartTypingCurrent();
    }

    private void StartTypingCurrent()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeLine(dialogueData.dialogueLines[index]));
    }

    private IEnumerator TypeLine(string full)
    {
        isTyping = true;
        if (dialogueText) dialogueText.text = string.Empty;

        float delay = Mathf.Max(0f, dialogueData.dialogueSpeed);

        for (int i = 0; i < full.Length; i++)
        {
            if (dialogueText) dialogueText.text += full[i];
            if (delay > 0f) yield return new WaitForSeconds(delay);
            else yield return null;
        }

        isTyping = false;

        // Auto-advance if flagged for this index
        if (dialogueData.autoProgressLines != null
            && dialogueData.autoProgressLines.Length == dialogueData.dialogueLines.Length
            && dialogueData.autoProgressLines[index])
        {
            yield return new WaitForSeconds(Mathf.Max(0f, dialogueData.autoProgressDelay));
            Next();
        }
    }

    private void FinishCurrentInstant()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = null;

        if (dialogueText) dialogueText.text = dialogueData.dialogueLines[index];
        isTyping = false;
    }

    private void Next()
    {
        if (index + 1 < dialogueData.dialogueLines.Length)
        {
            index++;
            StartTypingCurrent();
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = null;

        dialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (dialogueText != null) dialogueText.text = string.Empty;

        if (playerMovement != null) playerMovement.SetFrozen(false);
    }

    void Update()
    {
        if (!dialogueActive) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping) FinishCurrentInstant();
            else Next();
        }
    }
}