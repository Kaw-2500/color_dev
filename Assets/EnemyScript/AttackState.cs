using UnityEngine;

public class AttackState : IEnemyState // 敵の「攻撃」状態の具体的なロジックのみに責任を持つ
{
    private EnemyStateManager manager;
    private Enemy enemy => manager.GetEnemy();

    private float timer = 0f; // クラスフィールドとして保持
    private float CoolDowntimer => manager.GetEnemy().GetAttackCooldown();

    public AttackState(EnemyStateManager manager) => this.manager = manager;

    public void EnterState()
    {
        enemy.GetMovable().Stop(); // 攻撃中は移動を停止
    }

    public void UpdateState()
    {
        timer += Time.deltaTime;
        float dist = Vector2.Distance(enemy.transform.position, enemy.GetPlayer().position);

        if (dist > enemy.GetAttackRange()) // クールダウンが終わり、攻撃範囲外に出たら
            manager.SetState(new ChaseState(manager)); 
        else if (timer >= CoolDowntimer)
        {
            timer = 0;
            enemy.GetAttacker().Attack(); 
        }
    }

    public void ExitState() { }
}