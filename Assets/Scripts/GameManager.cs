using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int winScore = 5;
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI aiScoreText;
    [SerializeField] private BallController ball;

    private int playerScore;
    private int aiScore;

    void Awake()
    {
        Instance = this;
    }

    public void PlayerScored()
    {
        playerScore++;
        playerScoreText.text = playerScore.ToString();
        if (playerScore >= winScore)
            EndGame(true);
        else
            ball.ResetBall();
    }

    public void AIScored()
    {
        aiScore++;
        aiScoreText.text = aiScore.ToString();
        if (aiScore >= winScore)
            EndGame(false);
        else
            ball.ResetBall();
    }

    void EndGame(bool playerWon)
    {
        PlayerPrefs.SetInt("PlayerWon", playerWon ? 1 : 0);
        SceneManager.LoadScene("GameOver");
    }
}
