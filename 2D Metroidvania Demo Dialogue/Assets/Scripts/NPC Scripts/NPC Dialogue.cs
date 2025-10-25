using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Dialogue/NPC Dialogue")]
public class NPCDialogue : ScriptableObject
{
    [Header("Identity")]
    public string npcName;
    public Sprite npcPortrait;

    [Header("Lines")]
    [TextArea(2, 6)]
    public string[] dialogueLines;

    [Tooltip("Typing delay per character (seconds). 0 = instant.")]
    public float dialogueSpeed = 0.02f;

    [Header("Auto Progress")]
    [Tooltip("If provided and same length as dialogueLines, true means that line will auto-advance.")]
    public bool[] autoProgressLines;

    [Tooltip("Seconds to wait before auto advancing an autoProgress line.")]
    public float autoProgressDelay = 0.8f;
}