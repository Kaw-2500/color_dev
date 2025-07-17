using UnityEngine;

[CreateAssetMenu(fileName = "Enemydata", menuName = "Enemy/Enemydata")]
public class EnemyData : ScriptableObject
{
    public float attackRange = 1.5f;
    public float chaseRange = 20f;
    public float maxSpeed = 5f;
    public float moveForce = 10f;
    public float attackCooldown = 1f;

    public float normalAttackOffsetX = 0.5f; // 生成時のX軸オフセット

    public float normalAttackForce = 5f; 
    public float rightRayLength = 0.5f;
    public float leftRayLength = 0.5f;

    public GameObject NormalAttackPrefab;
}
