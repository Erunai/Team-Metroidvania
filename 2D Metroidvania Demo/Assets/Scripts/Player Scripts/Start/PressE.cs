using UnityEngine;

public class PressE : MonoBehaviour
{
    public PlayerController playerCon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCon.gameObject.SetActive(false); // Set playercon disabled on start
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerCon.SetActive(true);
            playerCon.
        }
    }
}
