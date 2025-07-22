using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]  // SpriteRenderer•K{
[RequireComponent(typeof(Enemy))]           // Enemy•K{
public class WhiteningReaction : MonoBehaviour, IReactionOnDamage
{
    private WhiteningManager whiteningManager;
    private SpriteRenderer spriteRenderer;
    private Enemy baseEnemy;

    private float currentHp;

    [SerializeField] private EnemyData enemyData;

    [SerializeField] private WhiteCompleteEffectManager whiteCompleteEffect;

    public bool IsAttackDisabled { get; private set; } = false; // UŒ‚–³Œø‰»ƒtƒ‰ƒO

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyData == null)
        {
            Debug.LogError("[WhiteningReaction] enemyData‚ªƒZƒbƒg‚³‚ê‚Ä‚¢‚Ü‚¹‚ñB");
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

        // UŒ‚–³Œø‰»”»’è
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

        if (whiteningManager.CurrentStage == WhiteningManager.WhiteningStage.Complete)
        {
            IsAttackDisabled = true;

            if (whiteCompleteEffect != null)
            {
                whiteCompleteEffect.WhiteCompleteEffect(transform); // “G‚ÌˆÊ’u‚Å¶¬
                StartCoroutine(whiteCompleteEffect.FadeOutCoroutine(spriteRenderer, gameObject)); // “G‚Ì”j‰ó
            }
        }
        //Debug.Log($"[Whitening] Stage: {whiteningManager.CurrentStage}, Color: {spriteRenderer.color}");
    }
}
