using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public enum Scorer { Player, AI }
    [SerializeField] private Scorer scorer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball")) return;

        if (scorer == Scorer.Player)
            GameManager.Instance.PlayerScored();
        else
            GameManager.Instance.AIScored();
    }
}
