using System.Collections;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToRespawn;

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
        yield return new WaitForSeconds(waitToRespawn);
        PlayerController.instance.transform.position = CheckPointController.instance.spawnPoint;
        PlayerController.instance.gameObject.SetActive(true);

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        PlayerHealthController.instance.healthBar.SetHealth(PlayerHealthController.instance.currentHealth);
        if (!PlayerController.instance.canDash || PlayerController.instance.isDashing)
        {
            PlayerController.instance.isDashing = false;
            PlayerController.instance.canDash = true;
        }
    }
}
