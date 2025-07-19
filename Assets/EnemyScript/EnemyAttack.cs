using UnityEngine;

public class EnemyAttack : IAttackable
{
    private GameObject normalAttackPrefab;
    private Enemy enemy;
    private GameObject instance;
    private float attackPowerMultiplier = 1f;


    public EnemyAttack(GameObject prefab, Enemy enemy)
    {
        this.normalAttackPrefab = prefab;
        this.enemy = enemy;
    }
    public void SetAttackPowerMultiplier(float multiplier)
    {
        attackPowerMultiplier = multiplier;
    }



    public void Attack()
    {
        var direction = GetAttackDirection();
        var offsetX = direction == Vector2.right ? enemy.GetNormalAttackOffsetX() : -enemy.GetNormalAttackOffsetX();
        var spawnPos = enemy.transform.position + new Vector3(offsetX, 0, 0);
        instance = Object.Instantiate(normalAttackPrefab, spawnPos, Quaternion.identity);

        Vector3 localScale = instance.transform.localScale;
        localScale.x = direction == Vector2.right ? 1 : -1;
        instance.transform.localScale = localScale;   

        var comp = instance.GetComponent<IAttackComponent>();
        if (comp == null)
        {
            Debug.LogWarning("IAttackComponent‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñ");
            return;
        }
        comp.Init(direction,
                  enemy.GetNormalAttackForce(),
                  enemy.GetNormalAttackOffsetY(),
                  enemy.GetAttackPower() * attackPowerMultiplier);

        Debug.Log($"Enemy Attack! Power: {enemy.GetAttackPower() * attackPowerMultiplier}");

    }

    private Vector2 GetAttackDirection()
    {
        switch (enemy.GetPlayerRelativePosition())
        {
            case Enemy.PlayerRelativePosition.NearRight:
            case Enemy.PlayerRelativePosition.Right:
                return Vector2.right;
            case Enemy.PlayerRelativePosition.NearLeft:
            case Enemy.PlayerRelativePosition.Left:
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }
}
