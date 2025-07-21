using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]  // SpriteRenderer必須
[RequireComponent(typeof(Enemy))]           // Enemy必須
public class WhiteningReaction : MonoBehaviour, IReactionOnDamage
{
    private WhiteningManager whiteningManager;
    private SpriteRenderer spriteRenderer;
    private Enemy baseEnemy;

    private float currentHp;

    [SerializeField] private EnemyData enemyData;

    public bool IsAttackDisabled { get; private set; } = false; // 攻撃無効化フラグ

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyData == null)
        {
            Debug.LogError("[WhiteningReaction] enemyDataがセットされていません。");
            return;
        }

        whiteningManager = new WhiteningManager(enemyData.EnemyHp, spriteRenderer.color);
        baseEnemy = GetComponent<Enemy>();
    }

    void Start()
    {
        if (enemyData == null) return;

        currentHp = enemyData.EnemyHp;
        OnDamaged(currentHp);
    }

    public void OnDamaged(float currentHp)
    {
        whiteningManager.UpdateHp(currentHp);
        whiteningManager.UpdateWhiteColor(spriteRenderer);

        // 攻撃無効化判定
        if (whiteningManager.CurrentStage == WhiteningManager.WhiteningStage.Complete)
        {
            IsAttackDisabled = true;
        }

        var movable = baseEnemy.GetMovable();
        if (movable is EnemyMovement movementImpl)
            movementImpl.SetSpeedMultiplier(whiteningManager.SpeedMultiplier);

        var attacker = baseEnemy.GetAttacker();
        if (attacker is EnemyAttack attackImpl)
            attackImpl.SetAttackPowerMultiplier(whiteningManager.AttackMultiplier);

        Debug.Log($"[Whitening] Stage: {whiteningManager.CurrentStage}, Color: {spriteRenderer.color}");
    }
}
