using System.Collections;
using UnityEngine;

public class ThrownProjectile : MonoBehaviour
{
    /*
     * PseudoCode:
     * 
     * float distance = getDistanceToTarger();
     * float angle = calculateAngle(distance, height);
     * float initialVelocity = calculateInitialVelocity(distance, angle);
     * 
     * Instantiate(new Projectile(initialVelocity, angle), firePoint, 0f);
     */

    [SerializeField] Transform hidePos;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform target;
    [SerializeField] LineRenderer line;
    [SerializeField] float step;
    // [SerializeField] float maxSpeed = 1f; if we want velocity to be variable, we can use this

    [SerializeField] float attackTimer = 2f;
    private float attackCounter;

    private void Start()
    {
        attackCounter = attackTimer;
    }

    private void Update()
    {
        float angle;
        float initialVelocity;
        float time;

        Vector3 targetPos = target.position - firePoint.position;
        float height = targetPos.y / targetPos.magnitude / 2f; // TODO: Fix this formula
        height = Mathf.Max(Mathf.Epsilon, height);

        //time = Mathf.Pow((targetPos.x - firePoint.position.x) + (targetPos.y - firePoint.position.y), 1 / 2);
        //TODO: Set a clamp on Time

        CalculatePathWithHeight(targetPos, height, out initialVelocity, out angle, out time);
        //CalculatePath(targetPos, height, out initialVelocity, out angle);

        DrawPath(initialVelocity, angle, step); // Draw the path

        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }
        else
        {
            // Ensure that the counter is not less than the time taken for the projectile to reach the target
            attackCounter = Mathf.Max(attackTimer, time);

            Debug.Log("Initial Velocity: " + initialVelocity + " angle: " + angle + " time: " + time + " height: " + height);

            StopAllCoroutines();

            // Start the coroutine to move the projectile
            StartCoroutine(Coroutine_Movement(initialVelocity, angle, time));
        }
    }

    private float QuadraticEquation(float a, float b, float c, float sign)
    {
        return (-b + (sign * Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c) / (2 * a)));
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float initialVelocity, out float angle, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float b = Mathf.Sqrt(2 * g * h);
        float a = (-0.5f * g);
        float c = -yt;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);
        time = tplus > tmin ? tplus : tmin;

        angle = Mathf.Atan(b * time / xt);
        initialVelocity = b / Mathf.Sin(angle);
    }
    
    private void CalculatePath(Vector3 targetPos, float angle, out float initialVelocity, out float time)
    {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        initialVelocity = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (initialVelocity * Mathf.Cos(angle));

    }

    private void DrawPath(float initialVelocity, float angle, float step)
    {
        step = Mathf.Max(0.01f, step);
        float totalTime = 10;
        line.positionCount = (int)(totalTime / step) + 2;
        int count = 0;
        for(float t = 0; t < totalTime; t += step)
        {
            float x = initialVelocity * t * Mathf.Cos(angle);
            float y = initialVelocity * t * Mathf.Sin(angle) - ((1f / 2f) * -Physics.gravity.y * Mathf.Pow(t, 2));
            line.SetPosition(count, firePoint.position + new Vector3(x, y, 0));
            count++;
        }

        float xfinal = initialVelocity * totalTime * Mathf.Cos(angle);
        float yfinal = initialVelocity * totalTime * Mathf.Sin(angle) - ((1f / 2f) * -Physics.gravity.y * Mathf.Pow(totalTime, 2));
        line.SetPosition(count, firePoint.position + new Vector3(xfinal, yfinal, 0));
    }
    
    IEnumerator Coroutine_Movement(float initialVelocity, float angle, float time)
    {
        if (gameObject.activeInHierarchy.Equals(false)){
            gameObject.SetActive(true);
        }
        float t = 0;
        while (t < time)
        {
            float x = initialVelocity * t * Mathf.Cos(angle);
            float y = initialVelocity * t * Mathf.Sin(angle) - ((1f/2f) * -Physics.gravity.y * Mathf.Pow(t, 2));
            transform.position = firePoint.position + new Vector3(x, y, 0);
            t += Time.deltaTime;

            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            PlayerHealthController playerHealthController = collision.gameObject.GetComponent<PlayerHealthController>();
            playerHealthController.DamagePlayer();
        }
    }
}
