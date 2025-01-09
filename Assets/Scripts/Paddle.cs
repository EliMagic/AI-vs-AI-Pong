using UnityEngine;

public class Paddle : MonoBehaviour
{
    public float speed = 5f; // Speed of the paddle
    public bool isPlayerOne; // True for Player 1, False for Player 2
    private BoxCollider2D paddleCollider;

    void Start()
    {
        // Get the BoxCollider2D to calculate the paddle's height
        paddleCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        // Get the input for the paddle
        float move = Input.GetAxis(isPlayerOne ? "Vertical" : "Vertical2") * speed * Time.deltaTime;

        // Calculate new position
        float newY = transform.position.y + move;

        // Get paddle height dynamically
        float paddleHeight = paddleCollider.bounds.size.y;

        // Define limits based on wall positions and paddle height
        float topLimit = 5f - paddleHeight / 2; // Adjust for wall position
        float bottomLimit = -5f + paddleHeight / 2; // Adjust for wall position

        // Clamp the position within the screen bounds
        newY = Mathf.Clamp(newY, bottomLimit, topLimit);

        Debug.Log($"Top Limit: {topLimit}, Bottom Limit: {bottomLimit}, New Y: {newY}");

        // Update the paddle's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
