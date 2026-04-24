using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float speedIncrement = 0.5f;
    [SerializeField] private float maxSpeed = 20f;

    private Rigidbody2D rb;
    private float baseSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        baseSpeed = speed;
        Launch();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = rb.linearVelocity.normalized * speed;
    }

    void Launch()
    {
        float angle = Random.Range(-45f, 45f) * Mathf.Deg2Rad;
        float dirX = Random.value > 0.5f ? 1f : -1f;
        rb.linearVelocity = new Vector2(Mathf.Cos(angle) * dirX, Mathf.Sin(angle)) * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
            speed = Mathf.Min(speed + speedIncrement, maxSpeed);
    }

    public void ResetBall()
    {
        speed = baseSpeed;
        rb.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
        Invoke(nameof(Launch), 1f);
    }
}
