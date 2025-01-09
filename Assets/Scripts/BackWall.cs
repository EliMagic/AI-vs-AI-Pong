using UnityEngine;

public class BackWall : MonoBehaviour
{
    public GoalManager goalManager; // Reference to the GoalManager script
    public int scoringPlayer;       // 1 for Player 1's goal (Player 2 scores), 2 for Player 2's goal (Player 1 scores)

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            goalManager.AddPointToPlayer(scoringPlayer);
        }
    }
}
