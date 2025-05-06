using UnityEngine;

public class ParabolicPath : MonoBehaviour
{
    public static ParabolicPath instance;

    [SerializeField] private Vector3 firePoint;
    [SerializeField] private Vector3 targetPoint;
    private float gravity;

    private void Awake()
    {
        instance = this;
    }

    public ParabolicPath()
    {
        // Default constructor
        firePoint = Vector3.zero;
        targetPoint = Vector3.zero;
        gravity = -Physics.gravity.y; // Use the gravity value from Unity's Physics settings
    }

    public ParabolicPath(Vector3 firePoint, Vector3 targetPoint, float gravity)
    {
        gravity = -Physics.gravity.y; // Use the gravity value from Unity's Physics settings
        
        // Set variables
        this.firePoint = firePoint;
        this.targetPoint = targetPoint;
        this.gravity = gravity;
    }

    public Vector3 GetFirePoint()
    {
        return firePoint;
    }
    public Vector3 GetTargetPoint()
    {
        return targetPoint;
    }
    public float GetGravity()
    {
        return gravity;
    }

    public void GetParameters(out float initialVelocity, out float height, out float time, out float angle)
    {
        // Calculate the parameters of the parabolic path
        Vector3 targetPos = targetPoint - firePoint;
        height = targetPos.y / targetPos.magnitude / 2f; 
        height = Mathf.Max(Mathf.Epsilon, height);
        time = Mathf.Pow((targetPos.x - firePoint.x) + (targetPos.y - firePoint.y), 1 / 2);
        time = Mathf.Max(0.1f, time); // Ensure time is not less than a small value
        initialVelocity = Mathf.Sqrt(gravity * time * time / (2 * height));
        angle = Mathf.Atan(height / (targetPos.x - firePoint.x));
    }
}
