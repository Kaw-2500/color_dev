using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitDamage : MonoBehaviour
{
    [SerializeField] private Enemy enemy; // Enemyの参照
    [SerializeField] private CameraShaker shaker; // CameraShakerの参照
    [SerializeField] private Transform playertransform;

    [SerializeField] private float ParryAddforcePower = 10f;
    [SerializeField] private float ParryAddtorquePower = 500f;
    [SerializeField] private float CameraShakeStrength = 1f;
    Rigidbody2D rb2d;

    public void Awake()
    {
        enemy = GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("EnemyHitDamage must be attached to an Enemy GameObject.");
        }

        rb2d = GetComponent<Rigidbody2D>();
    }

    float ColorWeaknessDamageAmount(float damage, ThisColor color)
    {
        switch (color)
        {
            case ThisColor.Red:
                return damage * enemy.GetWeaknessRed();
            case ThisColor.Blue:
                return damage * enemy.GetWeaknessBlue();
            case ThisColor.Green:
                return damage * enemy.GetWeaknessGreen();
            default:
                return damage;  // どの色にも該当しなければ元ダメージを返す
        }
    }

    public void HitAttackDamageOnEnemy(float damage, ThisColor color)
    {
        Debug.Log($"EnemyHitDamage: 攻撃を食らった色 = {color}");
        damage = ColorWeaknessDamageAmount(damage, color);
        Debug.Log("弱点倍率補正後のダメージamount:" + damage);
        enemy.ApplyDamage(damage);
    }

    public void HitParryAttack()
    {
        Debug.Log("carsor: HitParryAttack");

        float relative = enemy.GetPlayerRelativeFloat(); // 1:プレイヤー右, -1:左
        float levity = enemy.GetEnemyLevity();
        float forcePower = levity * ParryAddforcePower;
        float torquePower = levity * ParryAddtorquePower;

        Vector2 force = new Vector2(-relative * forcePower * levity, forcePower * levity);
        rb2d.AddForce(force, ForceMode2D.Impulse);
        rb2d.AddTorque(relative * torquePower, ForceMode2D.Impulse);

        Debug.Log($"relative={relative}, force={force}, torque={relative * torquePower}");
        enemy.isKnockback = true;
    }
}
