using UnityEngine;

public class EnemyAttack : IAttackable // ◆SRP: IAttackableインターフェースを実装し、「攻撃」の具体的なロジックのみに責任を持つ
{
    private GameObject normalAttackPrefab; // 通常攻撃のプレハブを保持するフィールド
    private Enemy enemy; // Enemyの参照を保持するフィールド

    public EnemyAttack(GameObject normalprefab,Enemy enemycs)
    {
        normalAttackPrefab = normalprefab;
        enemy = enemycs; // Enemyの参照を保持
    }

    public void Attack()
    {
        Debug.Log("Enemy Attack!");

        Vector3 spawnPos = enemy.transform.position;
        float offsetX = 0f;
        Vector2 force = Vector2.zero;

        switch (enemy.GetPlayerRelativePosition())
        {
            case Enemy.PlayerRelativePosition.NearRight:
            case Enemy.PlayerRelativePosition.Right:
                offsetX = enemy.GetNormalAttackOffsetX();
                force = Vector2.right * enemy.GetNormalAttackForce();
                break;
            case Enemy.PlayerRelativePosition.NearLeft:
            case Enemy.PlayerRelativePosition.Left:
                offsetX = -enemy.GetNormalAttackOffsetX();
                force = Vector2.left * enemy.GetNormalAttackForce();
                break;
            default:
                Debug.LogWarning("EnemyAttack: プレイヤーの位置が不明");
                break;
        }

        spawnPos += new Vector3(offsetX, 0, 0);

        GameObject attackInstance = Object.Instantiate(normalAttackPrefab, spawnPos, Quaternion.identity);

        WindyAttack windy = attackInstance.GetComponent<WindyAttack>();
        if (windy == null)
        {
         Debug.LogWarning("EnemyAttack: WindyAttackコンポーネントが見つかりません。攻撃が正しく機能しない可能性があります。");
        }
        if (windy != null)
        {
            windy.SetForce(force);
        }
    }

}