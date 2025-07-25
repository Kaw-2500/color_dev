using UnityEngine;

[CreateAssetMenu(fileName = "Enemydata", menuName = "Enemy/Enemydata")]
public class EnemyData : ScriptableObject
{
    public float attackRange = 1.5f;
    public float chaseRange = 20f;
    public float maxSpeed = 5f;
    public float moveForce = 10f;
    public float attackCooldown = 1f;

    public float normalAttackOffsetX = 0.5f; // ¶¬‚ÌX²ƒIƒtƒZƒbƒg

    public float normalAttackOffsetY = 1.5f; 

    public float normalAttackForce = 5f; 
    public float rightRayLength = 0.5f;
    public float leftRayLength = 0.5f;

    public float EnemyHp = 100f; // “G‚ÌHP

    public float AttackPower = 10f; // “G‚ÌUŒ‚—Í

    public float Enemylevity = 20;     // ƒmƒbƒNƒoƒbƒN‚Ì‚Í‚¶‚©‚ê‚é‹­‚³(Œy‚³)(1‚Å’ÊíA1.5‚Å1.5”{‚ÌŒy‚³A0.5‚Å”¼•ª‚ÌŒy‚³)

    public GameObject NormalAttackPrefab;
}
