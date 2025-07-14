using UnityEngine;

public class PlayerMove : MonoBehaviour
    //動作のみを行います。
    //入力処理などはhandlerに任せます。
    //このクラスの役目は、物理演算の直接的な操作のみです。
{
    private Rigidbody2D rb2d;

    public Vector2 CurrentVelocity => rb2d.linearVelocity;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null) Debug.LogError("PlayerMove: Rigidbody2D���A�^�b�`����Ă��܂���");
    }

    public void HandleMove(float runForce, float maxSpeed, float gravityScale, float input)// input: -1 は左, 1 は右
    {
        rb2d.gravityScale = gravityScale;

        if (Mathf.Abs(rb2d.linearVelocity.x) < maxSpeed)
        {
            rb2d.AddForce(new Vector2(input * runForce, 0), ForceMode2D.Impulse);
        }
    }

    public void PlayerJump(float jumpForce,float gravityscale)
    {
      rb2d.gravityScale = gravityscale;

        rb2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        Debug.Log("PlayerMove: Player Jumped with force " + jumpForce);
        
    }
}
