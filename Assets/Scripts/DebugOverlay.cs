using UnityEngine;
using TMPro;

public class DebugOverlay : MonoBehaviour
{
    [SerializeField] private Rigidbody2D ballRb;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private KeyCode toggleKey = KeyCode.BackQuote;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            panel.SetActive(!panel.activeSelf);

        if (panel.activeSelf && ballRb != null)
            text.text = $"vel   {ballRb.linearVelocity.x:F2}  {ballRb.linearVelocity.y:F2}\nspeed  {ballRb.linearVelocity.magnitude:F2}";
    }
}
