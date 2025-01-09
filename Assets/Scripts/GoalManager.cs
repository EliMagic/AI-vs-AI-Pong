using TMPro;
using UnityEngine;
using UnityEngine.UI; // If you're using UI Text for displaying scores

public class GoalManager : MonoBehaviour
{
    public int player1Score = 0; // Player 1's score
    public int player2Score = 0; // Player 2's score

    public TextMeshProUGUI player1ScoreText; // Reference to Player 1's score UI Text
    public TextMeshProUGUI player2ScoreText; // Reference to Player 2's score UI Text

    public Transform ball;         // Reference to the ball's Transform
    public Rigidbody2D ballRb;     // Reference to the ball's Rigidbody2D
    public float resetDelay = 2f;  // Time delay before resetting the ball

    void Start()
    {
        UpdateScoreUI();
    }

    // Method to handle scoring
    public void AddPointToPlayer(int player)
    {
        if (player == 1)
        {
            player1Score++;
        }
        else if (player == 2)
        {
            player2Score++;
        }

        UpdateScoreUI();
        ResetBall();
    }

    // Update the score display
    void UpdateScoreUI()
    {
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    // Reset the ball to the center
    void ResetBall()
    {
        ballRb.velocity = Vector2.zero; // Stop the ball's movement
        ball.position = Vector3.zero;  // Reset the ball's position to the center

        // Launch the ball after a delay
        Invoke(nameof(LaunchBall), resetDelay);
    }

    // Launch the ball in a random direction
    void LaunchBall()
    {
        float xDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        float yDirection = Random.Range(0, 2) == 0 ? 1 : -1;
        ballRb.velocity = new Vector2(xDirection, yDirection).normalized * 10f; // Adjust speed as needed
    }
}
