using UnityEngine;

public class EnemyMovement : IMovable // ◆SRP: IMovableインターフェースを実装し、「移動」の具体的なロジックのみに責任を持つ
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

    public void Move(Vector2 direction) // IMovableインターフェースのMoveメソッドの実装
    {
        float speed = Mathf.Abs(rb.linearVelocity.x);
        if (speed < maxSpeed)
            rb.AddForce(new Vector2(force * Mathf.Sign(direction.x), 0), ForceMode2D.Force);

        // 敵の向きを移動方向に応じて変更
        Vector3 scale = rb.transform.localScale;
        scale.x = Mathf.Sign(direction.x);
        rb.transform.localScale = scale;
    }

    public void Stop() => rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.9f, rb.linearVelocity.y); // IMovableインターフェースのStopメソッドの実装
}