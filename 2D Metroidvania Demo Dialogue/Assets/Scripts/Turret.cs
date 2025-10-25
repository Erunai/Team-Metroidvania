using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bullet;
    [Space]
    [SerializeField] private float spawnTimer = 5f;

    private Quaternion rotation;

    private float spawnCounter;
    private Boolean facingLeft;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (transform.position.x > bulletSpawnPoint.position.x)
        {
            rotation = Quaternion.Euler(0, 0, 90f);
        }
        else
        {
            rotation = Quaternion.Euler(0, 0, -90f);
        }


        spawnCounter = spawnTimer;
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;

        if (spawnCounter < 0)
        {
            // Instantiate bullet
            Instantiate(bullet, bulletSpawnPoint.position, rotation);

            // Reset spawnCounter
            spawnCounter = spawnTimer;
        }
    }
}
