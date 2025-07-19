using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitDamage :MonoBehaviour
{

    [SerializeField] private Enemy enemy; // Enemyの参照

    public void Awake()
    {
        // Enemyコンポーネントを取得
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("EnemyHitDamage must be attached to an Enemy GameObject.");
        }
    }
    public void HitAttackDamageOnEnemy(float damage)
    {
        enemy.ApplyDamage(damage);
    }
}