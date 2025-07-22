using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]  // SpriteRenderer�K�{
[RequireComponent(typeof(Enemy))]           // Enemy�K�{
public class WhiteningReaction : MonoBehaviour, IReactionOnDamage
{
    private WhiteningManager whiteningManager;
    private SpriteRenderer spriteRenderer;
    private Enemy baseEnemy;

    private float currentHp;

    [SerializeField] private EnemyData enemyData;

    [SerializeField] private WhiteCompleteEffectManager whiteCompleteEffect;

    public bool IsAttackDisabled { get; private set; } = false; // �U���������t���O

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (enemyData == null)
        {
            Debug.LogError("[WhiteningReaction] enemyData���Z�b�g����Ă��܂���B");
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

        // �U������������
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
                whiteCompleteEffect.WhiteCompleteEffect(transform); // �G�̈ʒu�Ő���
                StartCoroutine(whiteCompleteEffect.FadeOutCoroutine(spriteRenderer, gameObject)); // �G�̔j��
            }
        }
        //Debug.Log($"[Whitening] Stage: {whiteningManager.CurrentStage}, Color: {spriteRenderer.color}");
    }
}
