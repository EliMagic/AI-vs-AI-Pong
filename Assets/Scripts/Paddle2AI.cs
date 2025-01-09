using UnityEngine;

public class Paddle2AI : MonoBehaviour
{
    [SerializeField] float baseSpeed; // Base speed of Player1's paddle
    public Transform ball; // Reference to the ball's Transform
    public Paddle1AI paddle1AI; 
    [SerializeField] float predictiveFactor; // Prediction factor to track future ball position
    [SerializeField] float errorMargin; // Random error margin for realistic AI
    [SerializeField] float reactionTime;
    [SerializeField] public float smoothing; // Smoothing factor for gradual motion (lower is smoother)
    [SerializeField] float stopMargin;    // Margin within which the paddle stops moving
    [SerializeField] float minSpeedFactor; // Minimum paddle speed factor (slower when ball is moving slowly)
    [SerializeField] float maxSpeedFactor; // Maximum paddle speed factor (faster when ball is moving quickly)

    private Rigidbody2D rb;
    private float paddleHeight;
    private bool isReturningToCenter = false; // True if Paddle2 is moving to the center
    private bool isWaitingAtCenter = false;   // True if Paddle2 is waiting at the center
    private bool ballHitByPaddle1 = false;    // True if Paddle1 has hit the ball

    void Start()
    {
        // Get the Rigidbody2D component and paddle height
        rb = GetComponent<Rigidbody2D>();
        paddleHeight = GetComponent<BoxCollider2D>().bounds.size.y;
    }

    void FixedUpdate()
    {
        if (isReturningToCenter)
        {
            MoveToCenter();
        }
        else if (isWaitingAtCenter)
        {
            rb.velocity = Vector2.zero;
        }
        else if (ball != null && ballHitByPaddle1)
        {
            MovePaddleSmoothly();
        }
        else
        {
            BallHitByPaddle1(); // Notify that Paddle1 has the ball
        }
    }

    void MovePaddleSmoothly()
    {
        // Predict the ball's future position based on its velocity
        Vector2 ballVelocity = ball.GetComponent<Rigidbody2D>().velocity;
        float predictedY = ball.position.y + (ballVelocity.y * predictiveFactor);

        // Add random error margin for realism
        predictedY += Random.Range(-errorMargin, errorMargin);

        // Clamp the predicted position within game bounds
        float topLimit = 5f - paddleHeight / 2;
        float bottomLimit = -5f + paddleHeight / 2;
        predictedY = Mathf.Clamp(predictedY, bottomLimit, topLimit);

        // Stop moving if the ball is within the stop margin
        if (Mathf.Abs(predictedY - rb.position.y) <= stopMargin)
        {
            rb.velocity = Vector2.zero; // Stop paddle movement
            return;
        }

        // Adjust paddle speed based on ball's vertical velocity
        float verticalSpeed = Mathf.Abs(ballVelocity.y); // Get the absolute vertical speed of the ball
        float speedFactor = Mathf.Lerp(minSpeedFactor, maxSpeedFactor, verticalSpeed / 10f); // Map speed to a factor
        float adjustedSpeed = baseSpeed * speedFactor;

        // Move Paddle2 toward the predicted position
        float step = adjustedSpeed * Time.fixedDeltaTime;
        float newY = Mathf.MoveTowards(rb.position.y, predictedY, step);
        rb.MovePosition(new Vector2(rb.position.x, newY));
    }

    void MoveToCenter()
    {
        // Move Paddle2 toward the center position (Y = 0)
        float step = baseSpeed * Time.fixedDeltaTime;
        float newY = Mathf.MoveTowards(rb.position.y, 0f, step);
        rb.MovePosition(new Vector2(rb.position.x, newY));

        // Stop moving once Paddle2 reaches the center
        if (Mathf.Approximately(rb.position.y, 0f))
        {
            isReturningToCenter = false;
            isWaitingAtCenter = true; // Wait at center until Paddle1 hits the ball
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // When Paddle1 hits the ball, notify Paddle2
            paddle1AI.BallHitByPaddle2();

            // When Paddle2 hits the ball, return to center
            isReturningToCenter = true;
            isWaitingAtCenter = false; // Reset waiting status
            ballHitByPaddle1 = false;  // Disable movement until Paddle1 hits the ball
        }
    }

    public void BallHitByPaddle1()
    {
        // Enable tracking when Paddle1 hits the ball
        ballHitByPaddle1 = true;
        isWaitingAtCenter = false; // Stop waiting at the center
    }
}