using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreTextPlayer1, scoreTextPlayer2;
    private int scorePlayer1 = 0, scorePlayer2 = 0;

    public void PlayerScored(bool isPlayerOne)
    {
        if (isPlayerOne)
            scorePlayer1++;
        else
            scorePlayer2++;

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreTextPlayer1.text = "Player 1: " + scorePlayer1;
        scoreTextPlayer2.text = "Player 2: " + scorePlayer2;
    }
}
