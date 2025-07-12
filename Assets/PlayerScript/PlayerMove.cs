using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public void HandleMove(float runForce, float maxSpeed, float jumpForce, float gravityScale, Rigidbody2D rb2d, float input)
    {
        rb2d.gravityScale = gravityScale;

        if (Mathf.Abs(rb2d.linearVelocity.x) < maxSpeed)
        {
            rb2d.AddForce(new Vector2(input * runForce, 0), ForceMode2D.Impulse);
        }
    }

    public void PlayerJump(float jumpForce, Rigidbody2D rb2d)
    {
        rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}
