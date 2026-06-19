using System.Collections;
using UnityEngine;

public class PaddlePunch : MonoBehaviour
{
    [SerializeField] private float punchAngle = 30f;
    [SerializeField] private float punchDuration = 0.08f;
    [SerializeField] private float snapDuration = 0.1f;
    [SerializeField] private float cooldown = 0.5f;

    public bool IsPunching { get; private set; }

    private Rigidbody2D rb;
    private bool onCooldown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (onCooldown) return;

        if (Input.GetKeyDown(KeyCode.A))
            StartCoroutine(DoPunch(-punchAngle));
        else if (Input.GetKeyDown(KeyCode.D))
            StartCoroutine(DoPunch(punchAngle));
    }

    IEnumerator DoPunch(float targetAngle)
    {
        onCooldown = true;
        IsPunching = true;

        float elapsed = 0f;
        while (elapsed < punchDuration)
        {
            rb.MoveRotation(Mathf.Lerp(0f, targetAngle, elapsed / punchDuration));
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        rb.MoveRotation(targetAngle);

        IsPunching = false;

        elapsed = 0f;
        while (elapsed < snapDuration)
        {
            rb.MoveRotation(Mathf.Lerp(targetAngle, 0f, elapsed / snapDuration));
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        rb.MoveRotation(0f);

        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }
}
