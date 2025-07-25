using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitDamage :MonoBehaviour
{

    [SerializeField] private Enemy enemy; // Enemy�̎Q��
    [SerializeField] private CameraShaker shaker;
    [SerializeField] private Transform playertransform;

    [SerializeField] private float CameraShakeStrength = 1f;
    Rigidbody2D rb2d;

    public void Awake()
    {
        // Enemy�R���|�[�l���g���擾
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
        float relative = enemy.GetPlayerRelativeFloat(); // �v���C���[���E�Ȃ�1�A���Ȃ�-1

        enemy.ApplyDamage(damageAmount * 3);
        Debug.Log("3�{�̃p���B�_���[�W��������");
    }

}