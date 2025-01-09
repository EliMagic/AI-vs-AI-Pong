using UnityEngine;

public class Ball : MonoBehaviour
{
    public float initialSpeed = 10f; // Initial speed of the ball
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        LaunchBall();
    }

    public void LaunchBall()
    {
        // Randomize initial direction
        float xDirection = Random.Range(0, 5) == 0 ? 1 : -1;
        float yDirection = Random.Range(0, 5) == 0 ? 1 : -1;

        // Set the initial velocity
        rb.velocity = new Vector2(xDirection, yDirection).normalized * initialSpeed;
    }

    void FixedUpdate()
    {
        // Maintain consistent speed
        rb.velocity = rb.velocity.normalized * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            // Determine the hit factor
            Vector2 paddlePosition = collision.transform.position;
            float hitFactor = (rb.position.y - paddlePosition.y) / collision.collider.bounds.size.y;

            // Calculate new direction
            float newX = Mathf.Sign(rb.velocity.x); // Keep current horizontal direction
            float newY = hitFactor * 2f; // Scale the vertical direction

            // Clamp the bounce angle to prevent extreme horizontal movement
            Vector2 newVelocity = new Vector2(newX, Mathf.Clamp(newY, -0.75f, 0.75f)).normalized * initialSpeed;

            // Apply the new velocity
            rb.velocity = newVelocity;
        }

        // Ensure the ball doesn't get stuck in purely vertical or horizontal movement
        PreventExtremeAngles();
    }

    void PreventExtremeAngles()
    {
        // Prevent horizontal lock
        if (Mathf.Abs(rb.velocity.x) < 0.5f)
        {
            float newX = rb.velocity.x > 0 ? 1f : -1f;
            rb.velocity = new Vector2(newX, rb.velocity.y).normalized * initialSpeed;
        }

        // Prevent vertical lock
        if (Mathf.Abs(rb.velocity.y) < 0.5f)
        {
            float newY = rb.velocity.y > 0 ? 1f : -1f;
            rb.velocity = new Vector2(rb.velocity.x, newY).normalized * initialSpeed;
        }
    }
}