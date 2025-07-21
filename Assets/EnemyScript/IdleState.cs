using UnityEngine;

public class IdleState : IEnemyState //敵の「待機」状態の具体的なロジックのみに責任を持つ
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy(); // 状態マネージャーからEnemyインスタンスを取得

    public IdleState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState() => enemy.GetMovable().Stop(); //IMovableインターフェース経由で移動を停止

    public void UpdateState()
    {
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);
        if (dist < enemy.GetChaseRange()) // プレイヤーが追跡範囲に入ったら
            manager.SetState(new ChaseState(manager)); //  状態遷移はEnemyStateManagerに依頼
    }

    public void ExitState() { }
}