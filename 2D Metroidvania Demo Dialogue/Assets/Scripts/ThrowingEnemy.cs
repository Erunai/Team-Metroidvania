using UnityEngine;

public class ThrowingEnemy : MonoBehaviour
{
    /*
     * Behaviour for the throwing enemy:
     *      When in range, the enemy will throw a projectile at the player
     *      If the player is in an outer range, it will try to move towards the player and then throw
     *      If the enemy is in a safe position, it will throw the projectile
     *      
     *      The projectile's path will use a RayCast to determine the optimal height and angle of the projectile
     *      The path will be drawn using a LineRenderer
     *      The path will be calculated from a different class.
     *      
     */

    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;

    ParabolicPath parabolicPath;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        parabolicPath = new ParabolicPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
