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
        var whiteningReaction = enemy.GetComponent<WhiteningReaction>();
        if (whiteningReaction != null && whiteningReaction.IsAttackDisabled)
        {
            Debug.Log("白化反応により完全浄化されたため、攻撃をreturn");
            return; 
        }

        var direction = GetAttackDirection();
        var offsetX = enemy.GetNormalAttackOffsetX();
        var spawnPos = enemy.transform.position + new Vector3(offsetX * direction, 0, 0);
        instance = Object.Instantiate(normalAttackPrefab, spawnPos, Quaternion.identity);

        Vector3 localScale = instance.transform.localScale;
        localScale.x = localScale.x * direction; // 方向に応じてスケールを調整
        instance.transform.localScale = localScale;

        var comp = instance.GetComponent<IAttackComponent>();
        if (comp == null)
        {
            Debug.LogWarning("IAttackComponentが見つかりません");
            return;
        }
        comp.Init(direction,
          enemy.GetNormalAttackForce(),
          enemy.GetAttackPower() * attackPowerMultiplier);

        Debug.Log($"Enemy Attack! Power: {enemy.GetAttackPower() * attackPowerMultiplier}");
    }

    private float GetAttackDirection()
    {
        switch (enemy.GetPlayerRelativePosition())
        {
            case Enemy.PlayerRelativePosition.NearRight:
            case Enemy.PlayerRelativePosition.Right:
                return 1;
            case Enemy.PlayerRelativePosition.NearLeft:
            case Enemy.PlayerRelativePosition.Left:
                return -1;
            default:
                return 1;// デフォルトは右方向
        }
    }
}
