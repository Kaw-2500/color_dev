public interface IEnemyPhysicsState // ◆OOP: 物理更新処理を必要とする「状態」が実装するインターフェース
{
    void FixedUpdateState(); // 物理更新ごとの処理
}