using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1.0f;
    private float counter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        counter = timeToDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            Destroy(gameObject);
        }
    }
}
