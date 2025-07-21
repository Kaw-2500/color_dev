// EnemyMovement.cs
using UnityEngine;

public class EnemyMovement : IMovable
{
    private Rigidbody2D rb;
    private float force;
    private float maxSpeed;
    private float speedMultiplier = 1f;


    public EnemyMovement(Rigidbody2D rb, float force, float maxSpeed)
    {
        this.rb = rb;
        this.force = force;
        this.maxSpeed = maxSpeed;
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    public void Move(Vector2 direction)
    {
        // speedMultiplierが0の場合は移動しない
        if (speedMultiplier <= 0.001f) //閾値
        {
            Stop(); // 念のため速度を0に
            return;
        }

        // 既存の移動ロジック
        if (direction == Vector2.up)
        {
            rb.AddForce(Vector2.up * force * 1, ForceMode2D.Force);
        }

        // 最大速度チェック（縦 or 横）
        if (rb.linearVelocity.magnitude < maxSpeed * speedMultiplier) // maxSpeedにもspeedMultiplierを適用
            rb.AddForce(direction.normalized * force * speedMultiplier, ForceMode2D.Force);

        // 左右移動のときだけ向きを変える
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            Vector3 scale = rb.transform.localScale;
            scale.x = Mathf.Sign(direction.x);
            rb.transform.localScale = scale;
        }
    }


    public void Stop() => rb.linearVelocity = Vector2.zero; // 完全に停止
    // 必要であれば、角速度も止める rb.angularVelocity = 0f;
}