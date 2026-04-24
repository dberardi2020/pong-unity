using UnityEngine;

public class AIPaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float wallY = 4.5f;
    [SerializeField] private float wallHalfHeight = 0.25f;
    [SerializeField] private float padding = 0.1f;
    [SerializeField] private Transform ball;

    private Rigidbody2D rb;
    private float yBound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float paddleHalfHeight = transform.localScale.y / 2f;
        yBound = wallY - wallHalfHeight - paddleHalfHeight - padding;
    }

    void FixedUpdate()
    {
        if (ball == null) return;
        float targetY = Mathf.Clamp(ball.position.y, -yBound, yBound);
        float newY = Mathf.MoveTowards(rb.position.y, targetY, speed * Time.fixedDeltaTime);
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}
