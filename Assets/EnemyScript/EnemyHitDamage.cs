using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitDamage :MonoBehaviour
{

    [SerializeField] private Enemy enemy; // Enemyの参照
    [SerializeField] private CameraShaker shaker;
    [SerializeField] private Transform playertransform;

    [SerializeField] private float ParryAddforcePower = 10f;
    [SerializeField] private float ParryAddtorquePower = 500f;
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
        Debug.Log("carsor: HitParryAttack");

        float relative = enemy.GetPlayerRelativeFloat(); // 1:プレイヤー右, -1:左
        float levity = enemy.GetEnemyLevity();
        // ParryAddforcePower/ParryAddtorquePowerは倍率として使う
        float forcePower = levity * ParryAddforcePower;
        float torquePower = levity * ParryAddtorquePower;

        Vector2 force = new Vector2(-relative * forcePower * levity, forcePower * levity);
        rb2d.AddForce(force, ForceMode2D.Impulse);
        rb2d.AddTorque(relative * torquePower, ForceMode2D.Impulse);

        Debug.Log($"relative={relative}, force={force}, torque={relative * torquePower}");
        enemy.isKnockback = true;
    }

}