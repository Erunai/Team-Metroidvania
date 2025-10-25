using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform player; // Target to follow
    public float parallaxFactorX; // Parallax effect intensity on the X-axis
    public float parallaxFactorY; // Parallax effect intensity on the Y-axis

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = FindAnyObjectByType<CameraController>().transform;
        // or get by tag:
        // player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Move background based on player's position and parallax factor, adjusting for time.deltaTime
        Vector3 newPosition = new Vector3(
            player.position.x * parallaxFactorX,
            player.position.y * parallaxFactorY,
            transform.position.z // Keep original Z position
        );
        transform.position = newPosition;
    }
}
