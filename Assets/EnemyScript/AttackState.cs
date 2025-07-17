using UnityEngine;

public class AttackState : IEnemyState // ◆SRP: 敵の「攻撃」状態の具体的なロジックのみに責任を持つ
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();
    private float attackCooldown = 1f; // 攻撃クールダウン
    private float timer = 0f; // 攻撃タイマー

    public AttackState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState()
    {
        timer = 0;
        enemy.GetMovable().Stop(); // 攻撃中は移動を停止
        enemy.GetAttacker().Attack(); // ◆OOP: IAttackableインターフェース経由で攻撃を実行
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);

        if (timer > attackCooldown && dist > enemy.GetAttackRange()) // クールダウンが終わり、攻撃範囲外に出たら
            manager.SetState(new ChaseState(manager)); // 追跡状態に戻る
    }

    public void ExitState() { }
}