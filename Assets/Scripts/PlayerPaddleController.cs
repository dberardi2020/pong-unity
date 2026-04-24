using UnityEngine;

public class PlayerPaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float wallY = 4.5f;
    [SerializeField] private float wallHalfHeight = 0.25f;
    [SerializeField] private float padding = 0.1f;

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
        float input = Input.GetAxisRaw("Vertical");
        float newY = Mathf.Clamp(rb.position.y + input * speed * Time.fixedDeltaTime, -yBound, yBound);
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }
}
