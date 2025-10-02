using System.Collections;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float WaitToRespawn;

    private float _playerScore;

    private void Awake()
    {
        instance = this;
    }

    public void RespawnPlayer()
    {
        // Respawn player at the last checkpoint
        StartCoroutine(RespawnCo());
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
    }

    public void AddScore(int scoreToAdd)
    {
        _playerScore += scoreToAdd;
        Debug.Log("Player Score = " + _playerScore);
        // UIManager.instance.UpdateScore(playerScore);
    }
}
