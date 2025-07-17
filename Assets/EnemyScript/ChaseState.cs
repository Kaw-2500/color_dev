using UnityEngine;

public class ChaseState : IEnemyState, IEnemyPhysicsState // ◆SRP: 敵の「追跡」状態の具体的なロジックのみに責任を持つ
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();

    public ChaseState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState() { }

    public void UpdateState()
    {
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);

        if (dist <= enemy.GetAttackRange()) // 攻撃範囲内に入ったら
            manager.SetState(new AttackState(manager));
        else if (enemy.IsClimbing()) // 登り条件を満たしたら
            manager.SetState(new ClimbState(manager));
        else if (dist > enemy.GetChaseRange()) // 追跡範囲外に出たら
            manager.SetState(new IdleState(manager));
    }

    public void FixedUpdateState() // ◆SRP: 物理更新に特化した移動ロジック
    {
        float dx = enemy.GetPlayer().position.x - enemy.transform.position.x;
        Vector2 dir = dx >= 0 ? Vector2.right : Vector2.left;
        enemy.GetMovable().Move(dir); // ◆OOP: IMovableインターフェース経由で移動
    }

    public void ExitState() => enemy.GetMovable().Stop(); // 状態を抜けるときに移動を停止
}