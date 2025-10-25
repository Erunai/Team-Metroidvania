using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    public bool onlyDamageOnce = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Debug.Log("Player Hit");
            PlayerHealthController.instance.DamagePlayer();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") && !onlyDamageOnce)
        {
            PlayerHealthController.instance.DamagePlayer();
        }
    }
}
