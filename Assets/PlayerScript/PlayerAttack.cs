using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] PlayerStateManager playerStateManager;

    void Awake()
    {
        if (playerStateManager == null)
        {
            playerStateManager = GetComponent<PlayerStateManager>();
            if (playerStateManager == null)
            {
                playerStateManager = GetComponentInParent<PlayerStateManager>();
            }
        }
    }
    public void NormalAttack(GameObject attackPrefab)
    {
        var playerPos = transform.position;        
        var attackObj = Instantiate(attackPrefab, playerPos, Quaternion.identity);


        var strategy = attackObj.GetComponent<IAttackStrategy>();
        if (strategy != null)
        {
            float direction;

            if (playerStateManager.mousePozition == PlayerStateManager.MousePozition.Left)
            {
                direction = -1;
            }
            else
            {
                direction = 1;
            }

            strategy.Execute(this.gameObject,direction);
        }
        else
        {
            Debug.LogWarning("IAttackStrategyがアタッチされていません: " + attackObj.name);
        }
    }
}
