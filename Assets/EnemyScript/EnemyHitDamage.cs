using Unity.VisualScripting;
using UnityEngine;

public class EnemyHitDamage :MonoBehaviour
{

    [SerializeField] private Enemy enemy; // Enemy�̎Q��

    public void Awake()
    {
        // Enemy�R���|�[�l���g���擾
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