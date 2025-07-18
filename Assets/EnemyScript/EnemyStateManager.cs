using UnityEngine;

public class EnemyStateManager // ◆SRP: 敵の「状態遷移」と「現在の状態の管理」のみに責任を持つ
{
    private IEnemyState currentState; // ◆OOP: インターフェース型で状態を保持し、具体的な状態クラスに依存しない
    private Enemy enemy; // 状態マネージャーが制御するEnemyインスタンス

    public EnemyStateManager(Enemy enemy) // コンストラクタでEnemyインスタンスを受け取る
    {
        this.enemy = enemy;
    }

    public void SetState(IEnemyState newState) // ◆SRP: 状態の切り替えロジックを集約
    {
        //Debug.Log($"State changed from {currentState?.GetType().Name ?? "null"} to {newState.GetType().Name}");
        currentState?.ExitState(); // 現在の状態の終了処理を呼び出す
        currentState = newState; // 新しい状態に設定
        currentState.EnterState(); // 新しい状態の開始処理を呼び出す
    }

    public void Update() => currentState?.UpdateState(); // ◆OOP: 現在の状態オブジェクトに更新処理を委譲（ポリモーフィズム）

    public void FixedUpdate() // ◆SRP: 物理更新処理をFixedUpdateに分離
    {
        // ◆OOP: インターフェースのキャストにより、物理更新が必要な状態のみを処理（ポリモーフィズム）
        if (currentState is IEnemyPhysicsState phys)
            phys.FixedUpdateState();
    }

    public Enemy GetEnemy() => enemy; // 管理対象のEnemyを状態クラスに提供
}