using UnityEngine;
using System.Collections;

public class BlueSlashManager : MonoBehaviour, IAttackComponent
{
    [SerializeField] private float damageAmount = 10f;

    private Collider2D playerCollider = null;
    private Rigidbody2D rb2d;
    private Enemy enemy;

    private bool isAttacked = false;
    private bool isCharged = true;

    private bool playerInTrigger = false;

    private ParryState parryState = ParryState.None;

    private enum ParryState
    {
        None,         // 初期状態：パリィ待機前
        Parryable,    // パリィ可能時間中
        Parried,      // パリィ成功
        ParryFailed   // パリィ失敗（時間経過）
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isCharged = true;
        isAttacked = false;
        parryState = ParryState.None;
    }

    public void Init(float direction, float force, float damage, Enemy enemy)
    {
        this.enemy = enemy;
        if (this.enemy != null)
        {
            this.enemy.OnDamagedByPlayer += OnEnemyDamagedByPlayer;
        }

        damageAmount = damage;
        isAttacked = false;
        isCharged = true;
        parryState = ParryState.None;

        rb2d.AddForce(Vector2.right * direction * force, ForceMode2D.Impulse);
    }

    private void OnDestroy()
    {
        if (enemy != null)
        {
            enemy.OnDamagedByPlayer -= OnEnemyDamagedByPlayer;
        }
    }

    // アニメーションイベントから呼ばれる
    public void Fncharge()
    {
        isCharged = false;

        Debug.Log("パリィを開始");
        StartCoroutine(ParryWindowCoroutine(0.4f));
    }

    private IEnumerator ParryWindowCoroutine(float duration)
    {
        parryState = ParryState.Parryable;
        yield return new WaitForSeconds(duration);

        if (parryState == ParryState.Parryable)
        {
            Debug.Log("パリィ時間終了 → 失敗");
            parryState = ParryState.ParryFailed;
        }
    }

    // アニメーション終了イベントから呼ばれる
    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    private void StartParry()
    {
        if (isCharged) return; // チャージ中はまだ発動しない
        if (parryState != ParryState.Parryable) return;

        Debug.Log("パリィ処理を開始！");
        parryState = ParryState.Parried;
        // パリィ成功時のエフェクトや処理をここに追加しても良い
    }

    private void OnEnemyDamagedByPlayer()
    {
        if (parryState == ParryState.Parryable)
        {
            Debug.Log("パリィ成功！Enemyから攻撃された時にパリィ判定を受け取りました。");
            StartParry();
        }
        else
        {
            Debug.Log("パリィ失敗：パリィ可能時間外に攻撃されました。");
        }
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
        if (!collision.CompareTag("Player")) return;

        playerInTrigger = true;
        playerCollider = collision;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInTrigger = false;
        playerCollider = null;
    }

    private void TryApplyDamage(Collider2D collision)
    {
        if (isAttacked) return;
        if (parryState == ParryState.Parried)
        {
            Debug.Log("パリィ成功中のためダメージ無効");
            return;
        }

        var hitDamage = collision.GetComponent<PlayerHitDamage>();
        if (hitDamage != null)
        {
            hitDamage.OnHitDamage(damageAmount);
            isAttacked = true;
        }
    }
}
