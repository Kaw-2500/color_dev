using UnityEngine;

// 敵が一定間隔でWindy（風弾）をプレイヤー方向へ攻撃するスクリプト
public class WindyAttack : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    Rigidbody2D rb2d;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        ShootWindy();
    }

    void Update()
    {
       
    }

    void ShootWindy()
    {
        switch (enemy.GetPlayerRelativePosition())
        {
            case Enemy.PlayerRelativePosition.NearRight:
            case Enemy.PlayerRelativePosition.Right:
                rb2d.AddForce(Vector2.right * enemy.GetNormalAttackForce(), ForceMode2D.Impulse);
                break;

            case Enemy.PlayerRelativePosition.NearLeft:
            case Enemy.PlayerRelativePosition.Left:
                rb2d.AddForce(Vector2.left * enemy.GetNormalAttackForce(), ForceMode2D.Impulse);
                break;
        }
    }
}
