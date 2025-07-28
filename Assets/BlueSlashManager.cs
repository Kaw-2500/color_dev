using UnityEngine;
using System.Collections;
public class BlueSlashManager : MonoBehaviour, IAttackComponent
{
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float parryTime = 0.8f;
    private Camerapos cameraController;

    private Collider2D playerCollider = null;
    private Rigidbody2D rb2d;
    private Enemy enemy;

    private bool isAttacked = false;
    private bool isCharged = true;

    private bool playerInTrigger = false;

    private ParryState parryState = ParryState.None;

    private enum ParryState
    {
        None,
        Parryable,
        Parried,
        ParryFailed
    }

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isCharged = true;
        isAttacked = false;
        parryState = ParryState.None;
    }

    void Start()
    {
       enemy.canmove = false;//攻撃中は動けない
        Debug.Log(enemy.canmove);

        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        if (mainCamera != null)
        {
            cameraController = mainCamera.GetComponent<Camerapos>();
        }
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
            enemy.canmove = true;
            enemy.OnDamagedByPlayer -= OnEnemyDamagedByPlayer;
        }
        
    }

    public void finishCharge()//animから呼ばれる
    {
        isCharged = false;
        parryState = ParryState.Parryable;

        StartCoroutine(ParrySlowMotion(parryTime));

        if (cameraController != null)
        {
            //プレイヤーが近くにいないときはカメラズームしない
            if (enemy.playerRelativePosition == Enemy.PlayerRelativePosition.Right) return;
            if(enemy.playerRelativePosition == Enemy.PlayerRelativePosition.Left) return;
            cameraController.StartParryZoom(parryTime);
        }
    }

    private IEnumerator ParrySlowMotion(float parryTime)
    {
        Time.timeScale = 0.3f;

        // 経過時間を直接計測する方式に変更
        float elapsed = 0f;
        while (elapsed < parryTime)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // パリィ状態の更新を先に行う
        if (parryState == ParryState.Parryable)
        {
            parryState = ParryState.ParryFailed;
        }

        Time.timeScale = 1f;
    }
    public void OnAnimationEnd()
    {
        Destroy(gameObject);
    }

    private void SuccessParry()
    {
        if (isCharged) return;
        if (parryState != ParryState.Parryable) return;

        parryState = ParryState.Parried;

        // 距離に関係なく必ずシェイク
        if (cameraController != null)
        {
            cameraController.ShakeCamera(0.5f, 1.0f, 30, 90);
            Debug.Log("carsor: call ShakeCamera from BlueSlashManager");
        }
        else if(cameraController == null)
        {
            Debug.Log("carsor: cameraController is null");
        }

        EnemyHitDamage enemyhitdamage = enemy.GetEnemyHitObject();
        enemyhitdamage.HitParryAttack();
    }

    private void OnEnemyDamagedByPlayer()
    {
        if (parryState == ParryState.Parryable)
        {
            Debug.Log("cusor:Load SuccessParry");
            SuccessParry();
        }
        else
        {
            parryState = ParryState.ParryFailed;
        }
    }

    private void Update()
    {
        // パリィ失敗状態を優先的にチェック
        if (parryState == ParryState.ParryFailed && playerCollider != null)
        {
            TryApplyDamage(playerCollider);
            parryState = ParryState.None; // 状態リセット
        }
        else if (playerInTrigger && !isAttacked && !isCharged)
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

        if (parryState == ParryState.Parryable || parryState == ParryState.Parried)
        {
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
