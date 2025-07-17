using UnityEngine;

public class EnemyAttack : IAttackable // ◆SRP: IAttackableインターフェースを実装し、「攻撃」の具体的なロジックのみに責任を持つ
{
    public void Attack() // IAttackableインターフェースのAttackメソッドの実装
    {
        Debug.Log("Enemy Attack!"); // 現時点では攻撃ログを出力するだけ
        // 将来的には、攻撃アニメーションの再生、ダメージ処理、SE再生などのロジックがここに追加される
    }
}