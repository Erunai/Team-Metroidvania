using UnityEngine;

public class FlyAway : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float timeToGo = 5f;
    [SerializeField] Transform flyAwayTarget;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _animator.SetTrigger("fly");
            // Fly in a parabolic arc to the target position
            Vector2 startPosition = transform.position;
            Vector2 targetPosition = flyAwayTarget.position;
            StartCoroutine(FlyInArc(startPosition, targetPosition, timeToGo));
        }
    }

    // FlyInArc Coroutine
    private System.Collections.IEnumerator FlyInArc(Vector2 start, Vector2 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            // Parabolic interpolation
            float height = 4 * (t - t * t); // Simple parabola
            Vector2 currentPosition = Vector2.Lerp(start, end, t) + new Vector2(0, height);
            _rigidbody2D.MovePosition(currentPosition);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _rigidbody2D.MovePosition(end);
        Destroy(gameObject); // Remove crow after flying away
    }
}
