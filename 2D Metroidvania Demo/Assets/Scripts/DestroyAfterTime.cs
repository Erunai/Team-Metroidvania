using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 1.0f;
    private float counter;


    // Constructor for adding script to GameObject programmatically
    public DestroyAfterTime(float timeToDestroy, float counter)
    {
        this.timeToDestroy = timeToDestroy;
        this.counter = counter;
    }
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
