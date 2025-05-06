using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlyingEnemyShooter : MonoBehaviour
{
    /*
     * This script is for a flying enemy that can shoot projectiles at the player.
     * The enemy can move in circles, left and right, both, or neigher.
     */
    public float moveSpeed;
    public bool moveInCircles = false;
    public bool moveLeftAndRight = false;
    public float moveDistance = 2f;
    public float left = 5;
    public float right = 5;
    public float waveMultiplier = 2f;

    [SerializeField] public float fireRange = 10f;
    public float fireRate = 1f;
    public GameObject projectile;
    public Transform firePoint;
    public Transform target;
    private bool inRange = false;
    private float fireRateCounter;

    [Header("Light Control")]
    public Light2D lightSource;
    public float lightIntensity = 1f;
    public bool lightOn;

    private Vector2 origin;
    
    private void Start()
    {
        origin = transform.position;
        lightSource.intensity = 0f;
        lightOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(fireRateCounter > 0)
        {
            fireRateCounter -= Time.deltaTime;
        }

        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            float distance = Vector2.Distance(transform.position, target.position);
            if (distance <= fireRange)
            {
                inRange = true;

            }
            else
            {
                inRange = false;
            }

            if (inRange)
            {
                if (fireRateCounter <= 0f)
                {
                    Shoot();
                    fireRateCounter = fireRate;
                }
                if (!lightOn)
                {
                    lightSource.intensity = lightIntensity;
                    lightOn = true;
                }
                
            }
            else
            {
                lightSource.intensity = 0f;
                lightOn = false;
            }
        }

        if (moveInCircles)
        {
            MoveInCircles();
        }
        if (moveLeftAndRight)
        {
            MoveLeftAndRight();
        }
    }
    void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, firePoint.position, transform.rotation);
    }

    void MoveInCircles()
    {
        // Move in a circular motion around the origin
        float x = origin.x + Mathf.Cos(Time.time * waveMultiplier *moveSpeed) * moveDistance;
        float y = origin.y + Mathf.Sin(Time.time * waveMultiplier * moveSpeed) * moveDistance;
        transform.position = new Vector2(x, y);
    }

    void MoveLeftAndRight()
    {
        // Move left and right around the origin
        float x = origin.x + Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        float y = transform.position.y;
        transform.position = new Vector2(x, y);
    }
}
