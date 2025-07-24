using UnityEngine;

public class BlueSlashManager : MonoBehaviour, IAttackComponent
{
    [SerializeField] private float damageAmount = 10f;

    private bool isAttacked = false;
    private bool isCharged = true;  // チャージ状態を管理するフラグ
    private bool playerInTrigger = false;
    private Collider2D playerCollider = null;

    private Rigidbody2D rb2d;

    bool InPlayerAttackTrigger = false;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isCharged = true;
        isAttacked = false;
    }

    public void Init(float direction, float force, float damage)
    {
        isAttacked = false;
        isCharged = true;  // 初期化時にチャージ状態をリセット

        damageAmount = damage;
        rb2d.AddForce(Vector2.right * direction * force, ForceMode2D.Impulse);
    }

    // AnimationEventから呼ばれる想定
    void Fncharge()
    {
        isCharged = false;
    }

    // AnimationEventから呼ばれる想定
    void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    void StartParry()
    {
        if(!isCharged) return;//攻撃を始めないとパリィはさせない


    }



    private void Update()
    {
        if (playerInTrigger && !isAttacked && !isCharged && playerCollider != null)
        {
            TryApplyDamage(playerCollider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var attackStrategy = collision.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            InPlayerAttackTrigger = true;
            Debug.Log("パリィするぜ！");
        }


        if (!collision.CompareTag("Player")) return;
        playerInTrigger = true;
        playerCollider = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // IAttackStrategyを実装したコンポーネントを持っていなければ無視
        var attackStrategy = collision.GetComponent<IAttackStrategy>();
        if (attackStrategy != null)
        {
            InPlayerAttackTrigger = false;Debug.Log("パリィ終了！");
        }

        if (!collision.CompareTag("Player")) return;
        playerInTrigger = false;
        playerCollider = null;
    }

    private void TryApplyDamage(Collider2D collision)
    {
        if (isAttacked) return;

        var hitDamage = collision.GetComponent<PlayerHitDamage>();
        if (hitDamage != null)
        {
            hitDamage.OnHitDamage(damageAmount);
            isAttacked = true;
        }
    }
}
