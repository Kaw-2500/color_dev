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
            enemy.OnDamagedByPlayer -= OnEnemyDamagedByPlayer;
        }
    }

    public void finishCharge()
    {
        isCharged = false;
        parryState = ParryState.Parryable;

        if (cameraController != null)
        {
            cameraController.StartParryZoom(parryTime);
        }

        StartCoroutine(ParrySlowMotion(parryTime));
    }

    private IEnumerator ParrySlowMotion(float parryTime)
    {
        Time.timeScale = 0.75f;

        // �o�ߎ��Ԃ𒼐ڌv����������ɕύX
        float elapsed = 0f;
        while (elapsed < parryTime)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // �p���B��Ԃ̍X�V���ɍs��
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
        // �p���B�������̏���

        EnemyHitDamage enemyhitdamage = enemy.GetEnemyHitObject();
        enemyhitdamage.HitParryAttack();//enemy���̏������N��
    }

    private void OnEnemyDamagedByPlayer()
    {
        if (parryState == ParryState.Parryable)
        {
            SuccessParry();
        }
        else
        {
            parryState = ParryState.ParryFailed;
        }
    }

    private void Update()
    {
        // �p���B���s��Ԃ�D��I�Ƀ`�F�b�N
        if (parryState == ParryState.ParryFailed && playerCollider != null)
        {
            TryApplyDamage(playerCollider);
            parryState = ParryState.None; // ��ԃ��Z�b�g
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
