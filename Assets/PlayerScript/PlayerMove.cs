using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public Vector2 CurrentVelocity => rb2d.linearVelocity;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null) Debug.LogError("PlayerMove: Rigidbody2D���A�^�b�`����Ă��܂���");
    }

    public void HandleMove(float runForce, float maxSpeed, float gravityScale, float input)
    {
        rb2d.gravityScale = gravityScale;

        if (Mathf.Abs(rb2d.linearVelocity.x) < maxSpeed)
        {
            rb2d.AddForce(new Vector2(input * runForce, 0), ForceMode2D.Impulse);
        }
    }

    public void PlayerJump(float jumpForce)
    {
        rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }
}
