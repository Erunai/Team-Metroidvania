using System.Collections;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float WaitToRespawn;

    private float _playerScore;

    [SerializeField] TMPro.TextMeshProUGUI scoreText;

    [SerializeField] PlayerController player;

    [SerializeField] private bool sitOnStart = true;

    private float _scoreTimer = 1f;
    private float _scoreCounter;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _scoreCounter = _scoreTimer;
        if (sitOnStart)
            player.Animator.SetTrigger("SitDown"); // Set player to sitting position at start
        _playerScore = 0;
        updateScore();
    }
    private void Update()
    {
        _scoreCounter -= Time.deltaTime; // Probably bad practice but works for now
    }

    public void RespawnPlayer()
    {
        // Respawn player at the last checkpoint
        StartCoroutine(RespawnCo());
        if (_scoreCounter > 0) return; // Prevent multiple deductions within the score timer
        AddScore(-1); // Deduct 1 point on respawn
        _scoreCounter = _scoreTimer; // Reset score counter
    }
    private IEnumerator RespawnCo()
    {
        PlayerController.instance.gameObject.SetActive(false);
        yield return new WaitForSeconds(WaitToRespawn);
        PlayerController.instance.transform.position = CheckPointController.instance.spawnPoint;
        PlayerController.instance.gameObject.SetActive(true);

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        PlayerHealthController.instance.healthBar.SetHealth(PlayerHealthController.instance.currentHealth);
        if (!PlayerController.instance.CanDash)
        {
            PlayerController.instance.CanDash = true;
        }
        PlayerController.instance.Animator.SetTrigger("StandUp"); // Set player to sitting position on respawn
    }

    public void AddScore(int scoreToAdd)
    {
        _playerScore += scoreToAdd;
        Debug.Log("Player Score = " + _playerScore);
        updateScore();
        // UIManager.instance.UpdateScore(playerScore);
    }

    void updateScore()
    {
        if (scoreText != null)
            scoreText.text = _playerScore.ToString();
    }
}
