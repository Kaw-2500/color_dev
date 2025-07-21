public interface IEnemyState //敵の「状態」が持つべき共通の振る舞いを定義するインターフェース
{
    void EnterState(); // 状態開始時の処理
    void UpdateState(); // 毎フレームの更新処理
    void ExitState(); // 状態終了時の処理
}