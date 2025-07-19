using UnityEngine;

public class EnemyMovement : IMovable //  IMovableインターフェースを実装し、「移動」の具体的なロジックのみに責任を持つ
{
    private Rigidbody2D rb;
    private float force;
    private float maxSpeed;

    public EnemyMovement(Rigidbody2D rb, float force, float maxSpeed) // コンストラクタで必要な依存関係
    {
        this.rb = rb;
        this.force = force;
        this.maxSpeed = maxSpeed;
    }

    public void Move(Vector2 direction)
    {
           if (direction == Vector2.up)
        {
            rb.AddForce(Vector2.up * force * 1, ForceMode2D.Force);
        }

        // 最大速度チェック（縦 or 横）
        if (rb.linearVelocity.magnitude < maxSpeed)
            rb.AddForce(direction.normalized * force, ForceMode2D.Force);

        // 左右移動のときだけ向きを変える
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            Vector3 scale = rb.transform.localScale;
            scale.x = Mathf.Sign(direction.x);
            rb.transform.localScale = scale;
        }
    }


    public void Stop() =>     rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, rb.linearVelocity.y);

}