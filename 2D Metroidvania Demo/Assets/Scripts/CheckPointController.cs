using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    public static CheckPointController instance;

    private CheckPoint[] checkPoints;

    public Vector3 spawnPoint;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        checkPoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);
        spawnPoint = PlayerController.instance.transform.position; // Set the spawn point to the player's current position at the start of the game
    }

    public void DeactivateCheckPoints()
    {
        foreach (CheckPoint checkPoint in checkPoints)
        {
            checkPoint.resetCheckPoint(); // Once we get visual feedback -- reset the checkpoint to its original state
        }
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        spawnPoint = newSpawnPoint;
    }
}
