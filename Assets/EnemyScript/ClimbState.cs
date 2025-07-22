using UnityEngine;

public class ClimbState : IEnemyState, IEnemyPhysicsState //敵の「登り」状態の具体的なロジックのみに責任を持つ
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();

    public ClimbState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState() { }

    public void UpdateState()
    {
        if (!enemy.IsClimbing()) // 登り条件を満たさなくなったら
            manager.SetState(new ChaseState(manager)); // 追跡状態に戻る
        else if (Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position) > enemy.GetChaseRange()) // プレイヤーが追跡範囲外に出たら
            manager.SetState(new IdleState(manager)); // 待機状態に戻る
    }

    public void FixedUpdateState()
    {
        if (enemy.IsWallUnder()) // 足元に壁がある場合
            enemy.GetMovable().Move(Vector2.up); //IMovableインターフェース経由で上方向に移動
        //Debug.Log($"ClimbState FixedUpdateState called. isClimbing={enemy.IsClimbing()}, isWallUnder={enemy.IsWallUnder()}");
    }

    public void ExitState()
    {
        if (enemy.GetMovable() is EnemyMovement movementImpl)
        {
            movementImpl.ResetYVelocity(); // 新しいメソッドを呼び出す
        }
        Debug.Log("ClimbState: ExitState - Y速度をリセットしました。");
    }
}