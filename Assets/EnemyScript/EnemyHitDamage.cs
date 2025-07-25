using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitDamage :MonoBehaviour
{

    [SerializeField] private Enemy enemy; // Enemyの参照
    [SerializeField] private CameraShaker shaker;
    [SerializeField] private Transform playertransform;

    [SerializeField] private float CameraShakeStrength = 1f;
    Rigidbody2D rb2d;

    public void Awake()
    {
        // Enemyコンポーネントを取得
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("EnemyHitDamage must be attached to an Enemy GameObject.");
        }

        rb2d = GetComponent<Rigidbody2D>();
    }
    public void HitAttackDamageOnEnemy(float damage)
    {
        enemy.ApplyDamage(damage);
    }

    public void HitParryAttack(float damageAmount)
    {
        float relative = enemy.GetPlayerRelativeFloat(); // プレイヤーが右なら1、左なら-1

        enemy.ApplyDamage(damageAmount * 3);
        Debug.Log("3倍のパリィダメージを挙げる");
    }

}